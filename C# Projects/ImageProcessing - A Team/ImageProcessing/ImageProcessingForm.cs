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
        string[] images;                //Stores file names of all images
        DropletImage[] dropletImages;   //Stores every DropletImage object

        Bitmap displayedImage;

        public ImageProcessingForm()
        {
            InitializeComponent();
        }


        private void loadButton_Click(object sender, EventArgs e)
        {
            DialogResult result = loadImagesDialog.ShowDialog();               // Open folder dialog browser

            if (result == DialogResult.OK)
            {
                //Store file names of images within selected folder - both .TIF and .BMP files
                string[] tifImages = Directory.GetFiles(loadImagesDialog.SelectedPath, "*.tif");
                string[] bmpImages = Directory.GetFiles(loadImagesDialog.SelectedPath, "*.bmp");

                //Store all image file names within one array
                images = new string[tifImages.Length + bmpImages.Length];
                tifImages.CopyTo(images, 0);
                bmpImages.CopyTo(images, tifImages.Length);

                //Create a Droplet Image object for every given image
                dropletImages = new DropletImage[images.Length];
                runProgressBar.Maximum = images.Length;
                for (int i = 0; i < images.Length; i++)
                {
                    //Create the Droplet Image object
                    dropletImages[i] = new DropletImage(new Bitmap(images[i]), i);
                }

                //Display the number of files loaded in the status label
                statusLabel.Text = "Loaded " + images.Length + " images.";

                //Set displayed image to the fourth in the list
                dropletImages[4].createBlackWhiteImage();
                displayedImage = dropletImages[4].getBlackWhiteImage();

                //Set picturebox to black and white image
                currentImagePictureBox.Image = displayedImage;

            }
        }
    }
}
