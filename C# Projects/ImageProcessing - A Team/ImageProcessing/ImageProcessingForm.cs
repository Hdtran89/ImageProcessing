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
using System.Threading;

namespace ImageProcessing
{

    public partial class ImageProcessingForm : Form
    {
        string[] images;                //Stores file names of all images
        DropletImage[] dropletImages;   //Stores every DropletImage object
		
		
        bool[,] dropletMatrix;
        double baseToNeedleHeight; 
        Bitmap displayedImage;
        int frameRate = 100;

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

                //Display loading status
                statusLabel.Text = "Loading data...";

                //Create a Droplet Image object for every given image
                dropletImages = new DropletImage[images.Length];
                runProgressBar.Maximum = images.Length;
                for (int i = 0; i < images.Length; i++)
                {
                    //Create the Droplet Image object
                    dropletImages[i] = new DropletImage(new Bitmap(images[i]), i);
                }

                //Determine the location of the base and needle using test images
                int numTestImages = 5;
                //Always use the first and last images for convergence matrix
                dropletImages[0].CompareTestArea();
                dropletImages[dropletImages.Length - 1].CompareTestArea();
                //Evenly space out the remaining images being used for convergence matrix
                for (int i = 1; i < numTestImages - 1; i++)
                {
                    dropletImages[i * (dropletImages.Length / (numTestImages - 1))].CompareTestArea();
                }

                //Display the number of files loaded in the status label
                statusLabel.Text = "Loaded " + images.Length + " images.";
                DropletImage.ConvertFRtoSecPerImage(frameRate);
                
                //Set displayed image to the fourth in the list
                dropletImages[4].CreateBlackWhiteImage();
                displayedImage = dropletImages[4].GetBlackWhiteImage();

                //Set the image box to display the isolated droplet of an image
                currentImagePictureBox.Image = dropletImages[4].GetDropImage();
                DropletImage.UpdateTimeElapsed();

                /* 5 centroid is wrong for my test set (anne and sanan) */
                dropletImages[5].CreateBlackWhiteImage();
                //Set the image box to display the isolated droplet of an image
                currentImagePictureBox.Image = dropletImages[5].GetDropImage();
                DropletImage.UpdateTimeElapsed();

                /*
                dropletImages[6].CreateBlackWhiteImage();

                //Set the image box to display the isolated droplet of an image
                currentImagePictureBox.Image = dropletImages[6].GetDropImage();
                
                */
                //Enable the 'Run' button and 'Calibrate' button
                runButton.Enabled = true;
                runToolStripMenuItem.Enabled = true;
                calibrateButton.Enabled = true;

            }
        }

        private void baseNeedleHeightTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void blackWhiteNumericUpDown_ValueChanged(object sender, EventArgs e)
        {

        }

        private void calibrateButton_Click(object sender, EventArgs e)
        {

            int greyscaleThreshold = (int)blackWhiteNumericUpDown.Value;
            
            //Set the greyscale threshold
            DropletImage.SetGreyScaleThreshold(greyscaleThreshold);
           
            //Set displayed image to the fourth in the list and adjust according to new calibration value
            dropletImages[4].CreateBlackWhiteImage();
            displayedImage = dropletImages[4].GetBlackWhiteImage();

            //currentImagePictureBox.Image = null;
            //Set picturebox to black and white image
            currentImagePictureBox.Image = displayedImage;
            currentImagePictureBox.Refresh();
            
        }

        private void runButton_Click(object sender, EventArgs e)
        {
            frameRate = (int)frameRateNumericUpDown.Value;
            DropletImage.ConvertFRtoSecPerImage(frameRate);
			//Number of total threads
			int numThreads = 4;
			
			//Initialize threads
			Thread[] threads = new Thread[numThreads];
			
			//Assign image ranges to each thread
            int[] threadingStartIndices = new int[numThreads];
            int[] threadingEndIndices = new int[numThreads];
			int imagesPerThread = dropletImages.Length / numThreads;
			for(int i = 0; i < numThreads - 1; i++){
                threadingStartIndices[i] = i * imagesPerThread;
                threadingEndIndices[i] = (i + 1) * imagesPerThread;
			}
			//The last thread will cover the last quarter of images along with any excess images
            threadingStartIndices[numThreads - 1] = threadingEndIndices[numThreads - 2];
			threadingEndIndices[numThreads - 1] = dropletImages.Length - 1;
			

			//Perform one operation on each image at a time
			
			//Determine centroids of each image using threads
			for(int i = 0; i < numThreads; i++){
				//Begin the thread's processing
				threads[i] = new Thread(() => threadedCentroidDetermination(threadingStartIndices[i], threadingEndIndices[i]));
				threads[i].Start();
			}
			
			//Wait until threads are all finished before continuing
			for(int i = 0; i < numThreads; i++){
				threads[i].Join();
			}
			
			//Determine velocities of each image
			for(int i = 0; i < numThreads; i++){
				//Begin the thread's processing
                threads[i] = new Thread(() => threadedVelocityDetermination(threadingStartIndices[i], threadingEndIndices[i]));
				threads[i].Start();
			}
			
			//Wait until threads are all finished before continuing
			for(int i = 0; i < numThreads; i++){
				threads[i].Join();
			}
			
			//Determine accelerations of each image
			for(int i = 0; i < numThreads; i++){				
				//Begin the thread's processing
                threads[i] = new Thread(() => threadedAccelerationDetermination(threadingStartIndices[i], threadingEndIndices[i]));
				threads[i].Start();
			}
			
			//Wait until threads are all finished before continuing
			for(int i = 0; i < numThreads; i++){
				threads[i].Join();
			}
			
			//Give all image data to the output
			Output output = new Output("fileName.xcl", dropletImages.Length);
			for(int i = 0; i < dropletImages.Length; i++){
				output.insertRow(dropletImages[i], i);
			}
			//Generate excel file
			output.generateExcel();
        }

		private void threadedCentroidDetermination(int startIndex, int endIndex){
			for(int i = startIndex; i < endIndex; i++){
				dropletImages[i].DetermineCentroid();
			}
		}
		
		private void threadedVelocityDetermination(int startIndex, int endIndex){
			for(int i = startIndex; i < endIndex; i++){
				dropletImages[i].DetermineVelocity();
			}
		}
		
		private void threadedAccelerationDetermination(int startIndex, int endIndex){
			for(int i = startIndex; i < endIndex; i++){
				dropletImages[i].DetermineAcceleration();
			}
		}
		
        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frameRateNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            frameRate = (int)frameRateNumericUpDown.Value;
            Console.WriteLine(frameRate);
        }
    }
}
