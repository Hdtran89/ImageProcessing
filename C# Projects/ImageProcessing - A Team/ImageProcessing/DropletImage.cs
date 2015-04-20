using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ImageProcessing
{
    public struct Coord{
        public int xCoord;
        public int yCoord;
    }

    class DropletImage
    {
        //=============================================================
        //================    Class Properties    =====================
        //=============================================================

        //=== Image files ===
        Bitmap realImage;           //The image file as it appears in the data set
        Bitmap blackWhiteImage;     //The image file after it is converted to black and white

        //=== Image information ===
        int imageIndex;                     //The index of this image in the data set
        static int greyScaleThreshold = 32; //The value used to determine if a pixel will be white or black
        bool[,] blackWhiteMatrix;           //Matrix describing whether or not each pixel is black
        static bool[,] convergenceMatrix;   //Location of the base/needle in every image

        //=== Droplet information ===

        //Centroid variables
        int pixelThreshold = 3;
        List<Coord> circumferencePoints;

        double centroidX;           //The droplet centroid's X position (in pixels)
        double centroidY;           //The droplet centroid's Y position (in pixels)

        double realCentroidX;       //The droplet centroid's X position (in cm - if baseToNeedle height given) else still in pixels
        double realCentroidY;       //The droplet centroid's Y position (in cm - if baseToNeedle Height given) else still in pixels

        double prevCentroidX;
        double prevCentroidY;

        //Velocity variables
        double velocityX;           //The droplet's current X velocity in relation to the previous image (in pixels)
        double velocityY;           //The droplet's current Y velocity in relation to the previous image (in pixels)
        double velocityNet;         //The droplet's current net velocity in relation to the previous image (in pixels)

        double prevVelocityX;
        double prevVelocityY;

        //Acceleration variables
        double accelerationX;       //The droplet's current X acceleration in relation to the previous image (in pixels)
        double accelerationY;       //The droplet's current Y acceleration in relation to the previous image (in pixels)
        double accelerationNet;     //The droplet's current net acceleration in relation to the previous image (in pixels)

        //Time variables
        static double secondsPerImage;  //The number of seconds that elapse between two adjacent images
        double time;   //The number of seconds elapsed since the first image, which occurs at 0 seconds

        //=== Unit conversion information ===
        static double cmPerPixel = 1; //The user-defined distance in real units from the needle to the base
        static string unit = "px";
        double volume;

        bool[,] dropletMatrix;
        Bitmap dropImage;
        //=============================================================
        //=================    Class Methods    =======================
        //=============================================================

        public DropletImage(Bitmap image, int index)
        {
            //Save the given input
            realImage = image;
            imageIndex = index;
            DetermineTime();
            dropImage = new Bitmap(realImage.Width, realImage.Height);
            //Initialize convergence matrix once
            if (index == 0)
            {
                convergenceMatrix = new bool[image.Width, image.Height];
                //Initialize every cell in the convergence matrix to true
                for (int y = 0; y < image.Height; y++)
                {
                    for (int x = 0; x < image.Width; x++)
                    {
                        convergenceMatrix[x, y] = true;
                    }
                }
            }

            //Initialize black/white matrix
            blackWhiteMatrix = new bool[image.Width, image.Height];
            circumferencePoints = new List<Coord>();

        }

        private void CreateBlackWhiteMatrix()
        {
            //Loop through each pixel in the image
            for (int y = 0; y < realImage.Height; y++)
            {
                for (int x = 0; x < realImage.Width; x++)
                {
                    blackWhiteMatrix[x, y] = false;
                    Color originalcolor = realImage.GetPixel(x, y);             //Grayscaling the pixel in question
                    int grayscale = (int)((originalcolor.R * .3) + (originalcolor.G * .59) + (originalcolor.B * .11));

                    //Create black/white image and save results in black/white matrix
                    if (grayscale < greyScaleThreshold)
                    {
                        blackWhiteMatrix[x, y] = true;
                    }
                    else
                    {
                        blackWhiteMatrix[x, y] = false;
                    }
                }
            }
        }

        //Compare the current convergence matrix to this image and update convergence matrix accordingly
        public void ClearConvergenceMatrix()
        {
            
            //Compare this matrix to the convergence matrix
            for (int y = 0; y < realImage.Height; y++)
            {
                for (int x = 0; x < realImage.Width; x++)
                {
                    convergenceMatrix[x, y] = false;
                }
            }
        }
        //Compare the current convergence matrix to this image and update convergence matrix accordingly
        public void CompareTestArea()
        {
            //Create the black/white matrix for this image
            CreateBlackWhiteMatrix();

            //Compare this matrix to the convergence matrix
            for (int y = 0; y < realImage.Height; y++)
            {
                for (int x = 0; x < realImage.Width; x++)
                {
                    //Adjust the convergence matrix if there are any differences
                    if (!(convergenceMatrix[x, y] == true && blackWhiteMatrix[x, y] == true))
                    {
                        //If a pixel is not black in both the convergence matrix and
                        //the black/white matrix, remove it from the convergence matrix
                        convergenceMatrix[x, y] = false;
                    }
                }
            }
        }

        //Determine the location of the droplet by determining the difference
        //between this image's black/white matrix and the convergence matrix
        private void IsolateDroplet()
        {
            dropletMatrix = new bool[realImage.Width, realImage.Height];

            //rows of convergence matrix
            for (int y = 0; y < convergenceMatrix.GetLength(1); y++)
            {
                //col
                for (int x = 0; x < convergenceMatrix.GetLength(0); x++)
                {
                    dropletMatrix[x, y] = false;
                    //if convergence matrix is false and dropletImage is true, then thats the drop
                    if (convergenceMatrix[x, y] == false && blackWhiteMatrix[x, y] == true)
                    {
                        dropletMatrix[x, y] = true;
                    }
                    else
                    {
                        dropletMatrix[x, y] = false;
                    }
                }
            }
        }

        //Sweep dropletMatrix cols to find possible dropCircumference points
        private void XDropletSweep()
        {
            //rows of dropletMatrix 
            for (int y = 0; y < dropletMatrix.GetLength(1); y++)
            {
                //col
                int x = 0;
                int leftX = 0;
                int rightX = 0;
                int leftPixelCnt = 0;
                int rightPixelCnt = 0;

                while (x < dropletMatrix.GetLength(0) && leftPixelCnt < pixelThreshold)
                {
                    //if dropletMatrix is true, increment the leftPixelCnt
                    if ((dropletMatrix[x, y] == true))
                    {
                        leftPixelCnt++;
                    }
                    else
                    {
                        leftPixelCnt = 0;
                    }
                    x++;
                }
                if (leftPixelCnt == pixelThreshold)
                {
                    leftX = x;
                }

                x = dropletMatrix.GetLength(0) - 1;
                while (x > 0 && rightPixelCnt < pixelThreshold)
                {
                    //if convergence matrix is false and dropletImage is true, then thats the drop
                    if ((dropletMatrix[x, y] == true))
                    {
                        rightPixelCnt++;

                    }
                    else
                    {
                        rightPixelCnt = 0;
                    }
                    x--;
                }
                if (rightPixelCnt == pixelThreshold)
                    rightX = x;

                if (leftX != 0 && rightX != 0)
                {
                    AddCircumferencePoint(leftX - pixelThreshold, y);
                    AddCircumferencePoint(rightX + pixelThreshold, y);
                    //fill in droplet
                    for (int i = leftX; i < rightX; i++)
                    {
                        dropletMatrix[i, y] = true;
                    }
                }
            }
        }

        //Sweep dropletMatrix rows to find possible dropCircumference points
        private void YDropletSweep()
        {
            //columns of dropletMatrix 
            for (int x = 0; x < dropletMatrix.GetLength(0); x++)
            {
                
                int topY = 0;
                int bottomY = 0;
                int topPixelCnt = 0;
                int bottomPixelCnt = 0;

                //rows
                int y = 0;
                //sweep from top to bottom until you run into continuous pixels that meet the threshold
                while (y < dropletMatrix.GetLength(1) && topPixelCnt < pixelThreshold)
                {
                    //if dropletMatrix is true, increment the leftPixelCnt
                    if ((dropletMatrix[x, y] == true))
                    {
                        topPixelCnt++;
                    }
                    else
                    {
                        topPixelCnt = 0;
                    }
                    y++;
                }
                if (topPixelCnt == pixelThreshold)
                {
                    topY = y;
                }

                //then sweep from bottom to top until you run into continuous pixels 
                y = dropletMatrix.GetLength(1) - 1;
                while (y > 0 && bottomPixelCnt < pixelThreshold)
                {
                    //if convergence matrix is false and dropletImage is true, then thats the drop
                    if ((dropletMatrix[x, y] == true))
                    {
                        bottomPixelCnt++;

                    }
                    else
                    {
                        bottomPixelCnt = 0;
                    }
                    y--;
                }
                if (bottomPixelCnt == pixelThreshold)
                    bottomY = y;

                //if top sweep and bottom sweep are successful, fill in points between them
                if (topY != 0 && bottomY != 0)
                {
                    //add the point after accounting for threshold check shifting
                    AddCircumferencePoint(x, topY - pixelThreshold);
                    AddCircumferencePoint(x, bottomY + pixelThreshold);

                    for (int i = topY; i < bottomY; i++)
                    {
                        dropletMatrix[x, i] = true;
                    }
                }
            }
        }
        //eliminate white center
        private void FillDroplet()
        {
            XDropletSweep();
            YDropletSweep();
        }

        //Add points BELIEVED to be part ouf droplet circumference - based on a threshold of surrounding pixes
        private void AddCircumferencePoint(int x, int y)
        {
            Coord tempPt;
            tempPt.xCoord = x;
            tempPt.yCoord = y;
            if (!circumferencePoints.Contains(tempPt))
            {
                circumferencePoints.Add(tempPt);
            }
        }

        //Remove any points that may have been added to circumferencePoints 
        //that do not actually comprise the droplet 
        private void RemoveOutliers()
        {
            bool[] markedRows = new bool[realImage.Height]; //array to hold all rows in image that may
                                                            //hold a circumference point or outlier
            bool[] markedCols = new bool[realImage.Width]; //array to hold all columns in image that may
                                                            //hold a circumference point or outlier

            foreach(Coord circumPoint in circumferencePoints)
            {
                //Mark all rows
                markedRows[circumPoint.yCoord] = true;

                //Mark all cols
                markedCols[circumPoint.xCoord] = true;
            }

            //Find max num contiguous slots in marked rows
            int minDropIndex = -1;
            int maxContiguous = 0;
            int currentCount = 0;
            for (int i = 0; i < realImage.Height; i++)
            {
                if (markedRows[i] == true)
                {
                    currentCount++;
                }
                if (markedRows[i] == false && currentCount > 0)
                {
                    if (currentCount > maxContiguous)
                    {
                        maxContiguous = currentCount;
                        minDropIndex = i - maxContiguous;
                    }
                        
                    currentCount = 0;
                }
            }

            //Remove outliers in marked rows
            List<Coord> pointsToRemove = new List<Coord>();
            foreach (Coord circumPoint in circumferencePoints)
            {
                //if yCoord in outside of the drop's presumed range, need to remove it from list of circumference points
                if (!(circumPoint.yCoord >= minDropIndex && circumPoint.yCoord <= minDropIndex + maxContiguous))
                {
                    pointsToRemove.Add(circumPoint);
                }
            }
            //remove row outlier points from list of circumference points
            foreach (Coord circumPoint in pointsToRemove)
            {
                circumferencePoints.Remove(circumPoint);
            }

            //Find max num contiguous slots in marked cols
            minDropIndex = -1;
            maxContiguous = 0;
            currentCount = 0;
            for (int i = 0; i < realImage.Width; i++)
            {
                if (markedCols[i] == true)
                {
                    currentCount++;
                }
                if (markedCols[i] == false && currentCount > 0)
                {
                    if (currentCount > maxContiguous)
                    {
                        maxContiguous = currentCount;
                        minDropIndex = i - maxContiguous;
                    }

                    currentCount = 0;
                }
            }

            //Remove outliers in marked cols
            pointsToRemove = new List<Coord>();
            foreach (Coord circumPoint in circumferencePoints)
            {
                if (!(circumPoint.xCoord >= minDropIndex && circumPoint.xCoord <= minDropIndex + maxContiguous))
                {
                    pointsToRemove.Add(circumPoint);
                }
            }
            foreach (Coord circumPoint in pointsToRemove)
            {
                circumferencePoints.Remove(circumPoint);
            }

        }

        public void PreprocessImage()
        {
            CreateBlackWhiteMatrix();
            IsolateDroplet();
            FillDroplet();
            RemoveOutliers();
        }

        //Calc centroid Using the sums of the x and y coordinates of the circumference points.
        public void DetermineCentroid()
        {
            foreach (Coord circumPoint in circumferencePoints)
            {
                centroidX += circumPoint.xCoord;
                centroidY += circumPoint.yCoord;
            }

            centroidX /= circumferencePoints.Count;
            centroidY /= circumferencePoints.Count;
            realCentroidX = centroidX * cmPerPixel;
            realCentroidY = centroidY * cmPerPixel;

        }

        //Calculate Velocity change between centroid distances of this and previous image
        public void DetermineVelocity()
        {
            Console.WriteLine("prevCentroidX: " + prevCentroidX + " prevCentroidY: " + prevCentroidY);
            if (time != 0)
            {
                velocityX = (realCentroidX - prevCentroidX)/ secondsPerImage;
                velocityY = (realCentroidY - prevCentroidY)/ secondsPerImage;
            }
            else
            {
                velocityX = 0;
                velocityY = 0;
            }

            velocityNet = Math.Sqrt(Math.Pow(velocityX, 2) + Math.Pow(velocityY, 2));

        }

        public void DetermineAcceleration()
        {
            if (time != 0)
            {
                accelerationX = (velocityX - prevVelocityX) / secondsPerImage;
                accelerationY = (velocityY - prevVelocityY) / secondsPerImage;
            }
            else
            {
                accelerationX = 0;
                accelerationY = 0;
            }

            accelerationNet = Math.Sqrt(Math.Pow(accelerationX, 2) + Math.Pow(accelerationY, 2));
        }

        public void DetermineVolume()
        {
            //initialize min and max
            int minX = realImage.Width;
            int maxX = 0;
            int minY = realImage.Height;
            int maxY = 0;

            double horizontalDiam, verticalDiam = 0;

            foreach (Coord circumPoint in circumferencePoints)
            {
                if (circumPoint.xCoord <= minX)
                    minX = circumPoint.xCoord;
                
                if (circumPoint.xCoord > maxX)
                    maxX = circumPoint.xCoord;

                if (circumPoint.yCoord <= minY)
                    minY = circumPoint.yCoord;

                if (circumPoint.yCoord > maxY)
                    maxY = circumPoint.yCoord;
            }

            horizontalDiam = (maxX - minX) * cmPerPixel;
            verticalDiam = (maxY - minY) * cmPerPixel;
            //Console.WriteLine("horizontal diam: " + horizontalDiam + " vertical diam: " + verticalDiam);
            double radius = ((horizontalDiam + verticalDiam) / 4) * cmPerPixel;
            volume = (4.0 / 3.0) * Math.PI * Math.Pow(radius, 3);
        }

        /* will not be called by threads - calculations determined individually - just for testing
        public void PerformCalculations()
        {
            DetermineCentroid();
            DetermineVelocity();
            Console.WriteLine("sec: " + time);
            Console.WriteLine("centX: " + realCentroidX + " centY: " + realCentroidY);
            Console.WriteLine("velX: " + velocityX + " velY: " + velocityY);
            DetermineAcceleration();
            Console.WriteLine("accX: " + accelerationX + " accY: " + accelerationY);
            DetermineVolume();
            Console.WriteLine("volume: " + volume);
        }
        */

        //Show just the complete drop and nothing else
        public Bitmap GetDropImage()
        {
            Console.WriteLine("sec: " + time);
            Console.WriteLine("centX: " + realCentroidX + " centY: " + realCentroidY);
            Console.WriteLine("velX: " + velocityX + " velY: " + velocityY);
            Console.WriteLine("accX: " + accelerationX + " accY: " + accelerationY);
            Console.WriteLine("volume: " + volume);
            
            for (int y = 0; y < realImage.Height; y++)
            {
                for (int x = 0; x < realImage.Width; x++)
                {
                    if (dropletMatrix[x, y] == true)
                    {
                        dropImage.SetPixel(x, y, Color.Red);
                    }
                }
            }

            foreach (Coord circumPoint in circumferencePoints)
            {
                //draw circumference points for testing
                dropImage.SetPixel(circumPoint.xCoord, circumPoint.yCoord, Color.Black);
            }

            //set centroid pixel to green
            dropImage.SetPixel((int)centroidX, (int)centroidY, Color.Green);
            //Console.Write("x centroid: " + centroidX + " yCentroid: " + centroidY);
            return dropImage;
        }

        //show the complete drop plus needle and base
        public Bitmap GetBlackWhiteImage()
        {
            Console.WriteLine("sec: " + time);
            Console.WriteLine("centX: " + realCentroidX + " centY: " + realCentroidY);
            Console.WriteLine("velX: " + velocityX + " velY: " + velocityY);
            Console.WriteLine("accX: " + accelerationX + " accY: " + accelerationY);
            Console.WriteLine("volume: " + volume);

            //Initialize the black and white image to the real image
            blackWhiteImage = realImage;
            //Lauded llama code to convert each pixel to black or white
            for (int y = 0; y < realImage.Height; y++)                          //rows (ypos) in bitmap
            {
                for (int x = 0; x < realImage.Width; x++)                       //columuns (xpos) in bitmap
                {
                    //Create black/white image based on greyscale changes shown in blackwhiteMatrix and
                    //filled drop
                    if (blackWhiteMatrix[x, y] == true || dropletMatrix[x, y] == true)
                    {
                        blackWhiteImage.SetPixel(x, y, Color.Red);
                    }
                }
            }
            //set centroid pixel to black in drop image
            blackWhiteImage.SetPixel((int)centroidX, (int)centroidY, Color.Green);

            return blackWhiteImage;
        }

        //show the complete drop plus needle and base
        public Bitmap GetConvergence()
        {
            //Initialize the black and white image to the real image
            blackWhiteImage = realImage;
            //Lauded llama code to convert each pixel to black or white
            for (int y = 0; y < realImage.Height; y++)                          //rows (ypos) in bitmap
            {
                for (int x = 0; x < realImage.Width; x++)                       //columuns (xpos) in bitmap
                {
                    //Create black/white image based on greyscale changes shown in blackwhiteMatrix and
                    //filled drop
                    if (convergenceMatrix[x, y] == true)
                    {
                        blackWhiteImage.SetPixel(x, y, Color.Red);
                    }
                }
            }
            //set centroid pixel to black in drop image
            blackWhiteImage.SetPixel((int)centroidX, (int)centroidY, Color.Green);

            return blackWhiteImage;
        }

        public static void SetGreyScaleThreshold(int input)
        {
            greyScaleThreshold = input;
        }

        public static void ConvertFRtoSecPerImage(int frameRate)
        {
            
            secondsPerImage = (double) 1 / (double) frameRate;
        }

        private void DetermineTime()
        {
            time = imageIndex * secondsPerImage;
        }

        //Not done
        //Use amount of pixels measured between bottom of needle and top of base in image 
        //to relate to this same distance in mm units in real setUp
        public static void ConvertPixelToMicron(double baseToNeedleInCM)
        {
            int minY = 0; //represents pixel y postion of bottom of needle
            int maxY = convergenceMatrix.GetLength(1) - 1; //represents top of base

            int needleX = 0; //represents x position where needle is at its lowest point.
            int y = 0;
            //columns of convergenceMatric
            for (int x = 0; x < convergenceMatrix.GetLength(0); x++)
            {
                //row
                bool atNeedle = true;
                //sweep from top to bottom 
                while (y < convergenceMatrix.GetLength(1) && atNeedle)
                {
                    //if looking at part of needle(convergenceMatrix[x, y] == true), update lowest part of needle (minY) 
                    //and increment to check if pixel below is also part of needle
                    //Note: this is based on assumption the entire needle is represented completely in the convergence matrix
                    if ((convergenceMatrix[x, y] == true))
                    {
                        minY = y;
                        needleX = x;
                        y++;
                    }
                    else //if not looking at part of needle - break while and check next column
                    {
                        atNeedle = false;
                    }
                }
                
            }
            //then sweep from bottom to top until you run into continuous pixels 
            y = convergenceMatrix.GetLength(1) - 1;
            bool atBase = true;
            while (y > 0 && atBase)
            {
                //looking from the bottom up, if convergence matrix is true- this is the base,
                //continue looking up until no more base is found - this is the y for the top of the base
                //Note: this is based on assumption the entire base is represented completely in the convergence matrix
                if ((convergenceMatrix[needleX, y] == true))
                {
                    maxY = y;
                    y--;
                }
                else
                {
                    atBase = false;
                }
                
            }

            int baseToNeedleInPixels = maxY - minY;
            if (baseToNeedleInCM == -1)
            {
                cmPerPixel = 1;
                unit = "px";
            }
            else
            {
                cmPerPixel = baseToNeedleInCM / baseToNeedleInPixels;
                unit = "cm";
            }
      
            Console.WriteLine("cm per pixel: " + cmPerPixel);
        }

        public double GetTime()
        {
            DetermineTime();
            return time;
        }

        public double GetXCentroid()
        {
            return realCentroidX;
        }

        public double GetYCentroid()
        {
            return realCentroidY;
        }

        public double GetXVelocity()
        {
            return velocityX;
        }

        public double GetYVelocity()
        {
            return velocityY;
        }

        public double GetNetVelocity()
        {
            return velocityNet;
        }

        public double GetXAcceleration()
        {
            return accelerationX;
        }

        public double GetYAcceleration()
        {
            return accelerationY;
        }

        public double GetNetAcceleration()
        {
            return accelerationNet;
        }

        public double GetVolume()
        {
            return volume;
        }

        public string GetUnit()
        {
            return unit;
        }

        public void SetPrevCentroidValues(double xCentroid, double yCentroid)
        {
            prevCentroidX = xCentroid;
            prevCentroidY = yCentroid;
        }

        public void SetPrevVelocityValues(double xVelocity, double yVelocity)
        {
            prevVelocityX = xVelocity;
            prevVelocityY = yVelocity;
        }
    }
}
