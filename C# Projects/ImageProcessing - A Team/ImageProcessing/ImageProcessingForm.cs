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
		
		bool[,] convergenceMatrix;
        bool[,] dropletMatrix;
        double baseToNeedleHeight; 
        Bitmap displayedImage;
        int frameRate = 0;

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

                //Display the number of files loaded in the status label
                statusLabel.Text = "Loaded " + images.Length + " images.";

                //Set displayed image to the fourth in the list
                dropletImages[4].CreateBlackWhiteImage();
                displayedImage = dropletImages[4].GetBlackWhiteImage();
                //Set picturebox to black and white image
                //currentImagePictureBox.Image = displayedImage;

                //initialize the boolean area for determining test area
                convergenceMatrix = GetBooleanMatrix(displayedImage);

                //test 4 random images against initial to create the convergence matrix
                Random randomImageIndex = new Random();
                for (int x = 0; x < 4; x++)
                {
                    int index = randomImageIndex.Next(0,images.Length);
                    dropletImages[index].CreateBlackWhiteImage();
                    Bitmap testImage = dropletImages[index].GetBlackWhiteImage();
                    bool[,] tempTestMatrix = new bool[testImage.Width, testImage.Height];
                    tempTestMatrix = GetBooleanMatrix(testImage);
                    CompareTestArea(convergenceMatrix, tempTestMatrix);
                }
                
                //Test one image to get just the drop
                dropletImages[10].CreateBlackWhiteImage();
                Bitmap test1Image = dropletImages[10].GetBlackWhiteImage();
                bool[,] tempTest1Matrix = new bool[test1Image.Width, test1Image.Height];
                tempTest1Matrix = GetBooleanMatrix(test1Image);
                IsolateDroplet(convergenceMatrix, tempTest1Matrix);
                FillDroplet();
                Bitmap dropImage = new Bitmap(displayedImage.Width, displayedImage.Height);
                for (int y = 0; y < displayedImage.Height; y++)
                {
                    for (int x = 0; x < displayedImage.Width; x++)
                    {
                        if (dropletMatrix[x,y] ==true)
                        {
                            dropImage.SetPixel(x, y, Color.Red);
                        }
                        else
                        {
                            dropImage.SetPixel(x, y, Color.White);
                        }
                    }
                }

                //dropletImages[10].SetDropletMatrix(dropletMatrix);

                currentImagePictureBox.Image = dropImage;
                    

                //Enable the 'Run' button and 'Calibrate' button
                runButton.Enabled = true;
                runToolStripMenuItem.Enabled = true;
                calibrateButton.Enabled = true;

            }
        }
		
		//use black and white of entire image to create boolean matrix
        private bool[,] GetBooleanMatrix(Bitmap testImage)
        {

            bool[,] tempMatrix = new bool[testImage.Width, testImage.Height];
            for (int y = 0; y < testImage.Height; y++)
            {
                for (int x = 0; x < testImage.Width; x++)
                {
                    if (testImage.GetPixel(x, y) == Color.FromArgb(255, 255, 255, 255))
                    {
                        tempMatrix[x, y] = false;
                    }
                    else
                    {
                        tempMatrix[x, y] = true;
                    }
                }
            }
            return tempMatrix;
        }

        //compare boolean matrix of base/needle with another image
        private void CompareTestArea(bool[,] primaryMatrix, bool[,] testMatrix)
        {
            //Console.Write(primaryMatrix.GetLength(0));
            //rows of convergence matrix
            for (int y = 0; y < primaryMatrix.GetLength(1); y++)
            {
                //col
                for (int x = 0; x < primaryMatrix.GetLength(0); x++)
                {
                    //if either primaryMatrix and testMatrix are false, then primaryMatrix is false
                    if (!(primaryMatrix[x, y] == true && testMatrix[x, y] == true))
                    {
                        primaryMatrix[x, y] = false;
                    }
                }
            }
        }

        //compare dropletimage matrix with convergence to isolate the drop
        private void IsolateDroplet(bool[,] convergenceMatrix, bool[,] dropletImageMatrix)
        {
            dropletMatrix = new bool[dropletImageMatrix.GetLength(0), dropletImageMatrix.GetLength(1)];

            //rows of convergence matrix
            for (int y = 0; y < convergenceMatrix.GetLength(1); y++)
            {
                //col
                for (int x = 0; x < convergenceMatrix.GetLength(0); x++)
                {
                    //if convergence matrix is false and dropletImage is true, then thats the drop
                    if ((convergenceMatrix[x, y] == false && dropletImageMatrix[x, y] == true))
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

                x = dropletMatrix.GetLength(0)-1;
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

        private void baseNeedleHeightTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void blackWhiteNumericUpDown_ValueChanged(object sender, EventArgs e)
        {

        }

        private void calibrateButton_Click(object sender, EventArgs e)
        {
            //currentImagePictureBox.Image = null;
            //currentImagePictureBox.Invalidate();
            int greyscaleThreshold = (int)blackWhiteNumericUpDown.Value;
            
            if (greyscaleThreshold != 32)
            {
                //Set displayed image to the fourth in the list and adjust according to new calibration value
                dropletImages[4].CreateBlackWhiteImage(greyscaleThreshold);
                displayedImage = dropletImages[4].GetBlackWhiteImage();

                //currentImagePictureBox.Image = null;
                //Set picturebox to black and white image
                currentImagePictureBox.Image = displayedImage;
                currentImagePictureBox.Refresh();
            }
        }

        private void runButton_Click(object sender, EventArgs e)
        {
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
