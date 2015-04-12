using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessing
{
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
        double centroidX;           //The droplet centroid's X position (in pixels)
        double centroidY;           //The droplet centroid's Y position (in pixels)

        //Velocity variables
        double velocityX;           //The droplet's current X velocity in relation to the previous image (in pixels)
        double velocityY;           //The droplet's current Y velocity in relation to the previous image (in pixels)
        double velocityNet;         //The droplet's current net velocity in relation to the previous image (in pixels)

        //Acceleration variables
        double accelerationX;       //The droplet's current X acceleration in relation to the previous image (in pixels)
        double accelerationY;       //The droplet's current Y acceleration in relation to the previous image (in pixels)
        double accelerationNet;     //The droplet's current net acceleration in relation to the previous image (in pixels)

        //Time variables
        static double secondsPerImage;  //The number of seconds that elapse between two adjacent images
        double secondsElapsed;          //The number of seconds elapsed since the first image, which occurs at 0 seconds

        //=== Unit conversion information ===
        static double baseNeedleHeight; //The user-defined distance in real units from the needle to the base

        double volume;

        bool[,] dropletMatrix;
        //=============================================================
        //=================    Class Methods    =======================
        //=============================================================

        public DropletImage(Bitmap image, int index)
        {
            //Save the given input
            realImage = image;
            imageIndex = index;
            
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
                    }
                    else
                    {
                        blackWhiteImage.SetPixel(x, y, Color.White);
                    }
                }
            }
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

        //eliminate white center
        private void FillDroplet()
        {

            int constantThreshold = 3;
            //rows of dropletMatrix 
            for (int y = 0; y < dropletMatrix.GetLength(1); y++)
            {
                //col
                int x = 0;
                int leftX = 0;
                int rightX = 0;
                int leftPixelCnt = 0;
                int rightPixelCnt = 0;
                while (x < dropletMatrix.GetLength(0) && leftPixelCnt < constantThreshold)
                {
                    //if convergence matrix is false and dropletImage is true, then thats the drop
                    if ((dropletMatrix[x, y] == true))
                    {
                        leftPixelCnt++;
                    }
                    x++;
                }
                if (leftPixelCnt == constantThreshold)
                    leftX = x;

                x = dropletMatrix.GetLength(0) - 1;
                while (x > 0 && rightPixelCnt < constantThreshold)
                {
                    //if convergence matrix is false and dropletImage is true, then thats the drop
                    if ((dropletMatrix[x, y] == true))
                    {
                        rightPixelCnt++;
                    }
                    x--;
                }

                if (rightPixelCnt == constantThreshold)
                    rightX = x;

                if (leftPixelCnt >= constantThreshold && rightPixelCnt >= constantThreshold)
                {
                    for (int i = leftX; i < rightX; i++)
                    {
                        dropletMatrix[i, y] = true;
                    }
                }
            }
        }

        public Bitmap GetDropImage()
        {
            CreateBlackWhiteMatrix();
            IsolateDroplet();
            FillDroplet();
            Bitmap dropImage = new Bitmap(realImage.Width, realImage.Height);
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

            return dropImage;
        }

		public void DetermineCentroid()
        {

        }

        public void DetermineVelocity()
        {

        }

        public void DetermineAcceleration()
        {

        }

        public Bitmap GetBlackWhiteImage()
        {
            return blackWhiteImage;
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

        public void CalculateVolume(double horizontalDiam, double verticalDiam)
        {
            double radius = (horizontalDiam + verticalDiam) / 4;
            volume = (4 / 3) * Math.PI * Math.Pow(radius, 3);
        }

        public static void SetGreyScaleThreshold(int input)
        {
            greyScaleThreshold = input;
        }
    }
}
