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
        //Class Properties
        Bitmap realImage;           //The image file as it appears in the data set
        Bitmap blackWhiteImage;     //The image file after it is converted to black and white
        int imageIndex;             //The index of the image in the data set


        public DropletImage(Bitmap image, int index)
        {
            //Save the given input
            realImage = image;
            imageIndex = index;
        }

        public void createBlackWhiteImage()
        {
            //Initialize the black and white image to the real image
            blackWhiteImage = realImage;

            //Lauded llama code to convert each pixel to black or white
            for (int y = 0; y < realImage.Height; y++)                          //rows (ypos) in bitmap
            {

                for (int x = 0; x < realImage.Width; x++)                       //columuns (xpos) in bitmap
                {
                    Color originalcolor = realImage.GetPixel(x, y);             //Grayscaling the pixel in question
                    int grayscale = (int)((originalcolor.R * .3) + (originalcolor.G * .59) + (originalcolor.B * .11));

                    if (grayscale < 32)
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

        public Bitmap getBlackWhiteImage()
        {
            return blackWhiteImage;
        }

    }
}
