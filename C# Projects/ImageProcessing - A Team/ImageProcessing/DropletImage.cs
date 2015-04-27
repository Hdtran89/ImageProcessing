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
        int imageIndex;             //The index of this image in the data set

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
        }

        public void CreateBlackWhiteImage(int optionalGreyScaleThreshold = 32)
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

                    if (grayscale < optionalGreyScaleThreshold)
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

        public void CalculateVolume(double horizontalDiam, double verticalDiam)
        {
            double radius = (horizontalDiam + verticalDiam) / 4;
            volume = (4 / 3) * Math.PI * Math.Pow(radius, 3);
        }

        public void SetDropletMatrix(bool[,] matrix)
        {
            dropletMatrix = matrix;
        }
    }
}
