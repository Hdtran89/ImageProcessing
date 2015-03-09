using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace ImageProcessing
{

    public partial class ImageProcessingForm : Form
    {
        string[] images; //holds file names of all images
        bool[,] boolbasearray;

        Bitmap currentImage;

        public ImageProcessingForm()
        {
            InitializeComponent();
        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            DialogResult result = loadImagesDialog.ShowDialog();               // Open folder dialog browser

            if (result == DialogResult.OK)
            {
                //Store file names of images within selected folder
                images = Directory.GetFiles(loadImagesDialog.SelectedPath);

                //Set current image to the fourth in the list
                currentImage =new Bitmap(images[4]);

               // boolbasearray = new bool[currentImage.Width + 1, currentImage.Height + 1];   
       
                //Lauded llama code to convert each pixel to black or white
                for (int y = 0; y < currentImage.Height; y++)                          //rows (ypos) in bitmap
                {

                    for (int x = 0; x < currentImage.Width; x++)                       //columuns (xpos) in bitmap
                    {
                        Color originalcolor = currentImage.GetPixel(x, y);             //Grayscaling the pixel in question
                        int grayscale = (int)((originalcolor.R * .3) + (originalcolor.G * .59) + (originalcolor.B * .11));
                       
                        if (grayscale < 32) 
                        {
                            currentImage.SetPixel(x, y, Color.Black);
                        }
                        else
                        {
                            currentImage.SetPixel(x, y, Color.White);
                        }
                        
                    }
                }

                //Set picturebox to black and white image
                currentImagePictureBox.Image = currentImage;
                // Trying to Overlay black and white image on top of original image - failed
                /*
                Bitmap first = new Bitmap(images[14]);
                Bitmap finalImage = new Bitmap(first.Width,first.Height);
                //currentImage.MakeTransparent();
                using (Graphics g = Graphics.FromImage(finalImage))
                {
                   
                    g.DrawImage(first, new Rectangle(0,0,first.Width,first.Height));
                    //g.DrawImage(currentImage, new Rectangle(0,0,currentImage.Width,currentImage.Height));
                }
                //currentImage.MakeTransparent();

                currentImagePictureBox.Image = finalImage;
                 */
                
            }
        }
    }
}
