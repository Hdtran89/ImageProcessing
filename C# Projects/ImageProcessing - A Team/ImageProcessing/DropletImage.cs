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
        List<Coord> circumferencePoints;

        double centroidX;           //The droplet centroid's X position (in pixels)
        double centroidY;           //The droplet centroid's Y position (in pixels)

        static double prevCentroidX;
        static double prevCentroidY;

        //Velocity variables
        double velocityX;           //The droplet's current X velocity in relation to the previous image (in pixels)
        double velocityY;           //The droplet's current Y velocity in relation to the previous image (in pixels)
        double velocityNet;         //The droplet's current net velocity in relation to the previous image (in pixels)

        static double prevVelocityX;
        static double prevVelocityY;

        //Acceleration variables
        double accelerationX;       //The droplet's current X acceleration in relation to the previous image (in pixels)
        double accelerationY;       //The droplet's current Y acceleration in relation to the previous image (in pixels)
        double accelerationNet;     //The droplet's current net acceleration in relation to the previous image (in pixels)

        //Time variables
        static double secondsPerImage;  //The number of seconds that elapse between two adjacent images
        static double secondsElapsed;   //The number of seconds elapsed since the first image, which occurs at 0 seconds

        //=== Unit conversion information ===
        static double baseNeedleHeight; //The user-defined distance in real units from the needle to the base

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

        public void CreateBlackWhiteImage()
        {
            //Initialize the black and white image to the real image
            blackWhiteImage = realImage;
            //Console.Write("Hey " + optionalGreyScaleThreshold);
            //Lauded llama code to convert each pixel to black or white
            for (int y = 0; y < realImage.Height; y++)                          //rows (ypos) in bitmap
            {

                for (int x = 0; x < realImage.Width; x++)                       //columuns (xpos) in bitmap
                {
                    Color originalcolor = realImage.GetPixel(x, y);             //Grayscaling the pixel in question
                    int grayscale = (int)((originalcolor.R * .3) + (originalcolor.G * .59) + (originalcolor.B * .11));

                    //Create black/white image and save results in black/white matrix
                    if (grayscale < greyScaleThreshold)
                    {
                        blackWhiteImage.SetPixel(x, y, Color.Black);
                        //blackWhiteMatrix[x, y] = true;
                    }
                    else
                    {
                        blackWhiteImage.SetPixel(x, y, Color.White);
                        //blackWhiteMatrix[x, y] = false;
                    }
                }
            }
        }

        public Bitmap GetBlackWhiteImage()
        {
            return blackWhiteImage;
        }

        private void CreateBlackWhiteMatrix()
        {
            //Loop through each pixel in the image
            for (int y = 0; y < realImage.Height; y++)
            {
                for (int x = 0; x < realImage.Width; x++)
                {
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


        //Compare the current convergence matrix to this image
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
        private void CreateDropletMatrix()
        {

        }

        //Determine where the base and needle is located in every image
        public void DetermineBaseAndNeedle()
        {
            
        }

        //compare dropletimage matrix with convergence to isolate the drop
        private void IsolateDroplet()
        {
            dropletMatrix = new bool[realImage.Width, realImage.Height];

            //rows of convergence matrix
            for (int y = 0; y < convergenceMatrix.GetLength(1); y++)
            {
                //col
                for (int x = 0; x < convergenceMatrix.GetLength(0); x++)
                {
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

        int pixelThreshold = 3;

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
                    //draw outside circumference points ... but missing some, may be affecting centroid calc
                    dropImage.SetPixel(leftX - pixelThreshold, y, Color.Black);
                    dropImage.SetPixel(rightX + pixelThreshold, y, Color.Black);
                    for (int i = leftX; i < rightX; i++)
                    {
                        dropletMatrix[i, y] = true;
                    }
                }
            }
        }

        private void YDropletSweep()
        {
            //rows of dropletMatrix 
            for (int x = 0; x < dropletMatrix.GetLength(0); x++)
            {
                //col
                int y = 0;
                int topY = 0;
                int bottomY = 0;
                int topPixelCnt = 0;
                int bottomPixelCnt = 0;

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

                if (topY != 0 && bottomY != 0)
                {
                    AddCircumferencePoint(x, topY - pixelThreshold);
                    AddCircumferencePoint(x, bottomY + pixelThreshold);

                    //draw outside circumference points ... but missing some, may be affecting centroid calc
                    dropImage.SetPixel(x,topY - pixelThreshold, Color.Black);
                    dropImage.SetPixel(x,bottomY + pixelThreshold, Color.Black);
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
        public Bitmap GetDropImage()
        {
            CreateBlackWhiteMatrix();
            IsolateDroplet();
            FillDroplet();
            PerformCalculations();
            /*
            for (int y = 0; y < realImage.Height; y++)
            {
                for (int x = 0; x < realImage.Width; x++)
                {
                    if (dropletMatrix[x, y] == true)
                    {
                        dropImage.SetPixel(x, y, Color.Red);
                    }
                    else
                    {
                        dropImage.SetPixel(x, y, Color.White);
                    }
                }
            }
             * */
            //set centroid pixel to black
            dropImage.SetPixel((int)centroidX, (int)centroidY, Color.Black);

            return dropImage;
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
        }

        //Calculate Velocity change between centroid distances of this and previous image
        public void DetermineVelocity()
        {
            if (secondsElapsed != 0)
            {
                velocityX = (centroidX - prevCentroidX) / secondsPerImage;
                velocityY = (centroidY - prevCentroidY) / secondsPerImage;
            }
            else
            {
                velocityX = 0;
                velocityY = 0;
            }

            velocityNet = Math.Sqrt(Math.Pow(velocityX, 2) + Math.Pow(velocityY, 2));
            //make current centroid previous after calculation
            prevCentroidX = centroidX;
            prevCentroidY = centroidY;

        }

        public void DetermineAcceleration()
        {
            if (secondsElapsed != 0)
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

            //make current velocity previousVelocity after calculation
            prevVelocityX = velocityX;
            prevVelocityY = velocityY;
        }

        public void DetermineVolume(double horizontalDiam, double verticalDiam)
        {
            double radius = (horizontalDiam + verticalDiam) / 4;
            volume = (4 / 3) * Math.PI * Math.Pow(radius, 3);
        }

        public void PerformCalculations()
        {
            DetermineCentroid();
            DetermineVelocity();
            Console.WriteLine("sec: " + secondsElapsed);
            Console.WriteLine("centX: " + centroidX + " centY: " + centroidY);
            Console.WriteLine("velX: " + velocityX + " velY: " + velocityY);
            DetermineAcceleration();
            Console.WriteLine("accX: " + accelerationX + " accY: " + accelerationY);
        }

        public static void SetGreyScaleThreshold(int input)
        {
            greyScaleThreshold = input;
        }

        public static void ConvertFRtoSecPerImage(int frameRate)
        {
            
            secondsPerImage = (double) 1 / (double) frameRate;
        }

        public static void UpdateTimeElapsed()
        {
            secondsElapsed += secondsPerImage;
        }
    }
}
