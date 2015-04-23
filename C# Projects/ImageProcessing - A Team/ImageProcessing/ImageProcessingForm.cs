﻿using System;
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
using Excel = Microsoft.Office.Interop.Excel;

namespace ImageProcessing
{

    public partial class ImageProcessingForm : Form
    {
        string[] images;                //Stores file names of all images
        DropletImage[] dropletImages;   //Stores every DropletImage object

        Bitmap displayedImage;
        int frameRate = 100;
        double baseToNeedleHeight = 2; //cm
        string folderPath;
        LoadingWindow loadingWindow = new LoadingWindow();

        public ImageProcessingForm()
        {
            InitializeComponent();
            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.WorkerSupportsCancellation = true;
        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            DialogResult result = loadImagesDialog.ShowDialog();               // Open folder dialog browser

            if (result == DialogResult.OK)
            {
                //loadingWindow.Show();
                //folderPath = loadImagesDialog.SelectedPath;
                //Store file names of images within selected folder - both .TIF and .BMP files
                string[] tifImages = Directory.GetFiles(loadImagesDialog.SelectedPath, "*.tif");
                string[] bmpImages = Directory.GetFiles(loadImagesDialog.SelectedPath, "*.bmp");

                //Store all image file names within one array
                images = new string[tifImages.Length + bmpImages.Length];
                tifImages.CopyTo(images, 0);
                bmpImages.CopyTo(images, tifImages.Length);
                runProgressBar.Maximum = images.Length;

            }

            if (images.Length != 0)
            {
                 
                //Display loading status
                statusLabel.Text = "Loading data...";
                //Create a Droplet Image object for every given image
                dropletImages = new DropletImage[images.Length];
                for (int i = 0; i < images.Length; i++)
                {
                    //Create the Droplet Image object
                    dropletImages[i] = new DropletImage(new Bitmap(images[i]), i);
                }

                //Convert framesPerSec to seconds per image
                if (frameRateNumericUpDown.Value != 0)
                {
                    frameRate = (int)frameRateNumericUpDown.Value;
                }
                DropletImage.ConvertFRtoSecPerImage(frameRate);

                /* Create convergence matrix containing location of just needle and base */
                /* Based on Black/White Calibration value */
                CreateConvergenceMatrix();

                /* Use distance between base and needle in pixels 
                   and baseNeedleHeight in cm to calculate cm per pixel */
                ConvertPxToCm();
                
                //Display the number of files loaded in the status label
                statusLabel.Text = "Loaded " + images.Length + " images.";

                dropletImages[0].PreprocessImage();
                //dropletImages[1].PreprocessImage();
                dropletImages[0].DetermineCentroid();
                //dropletImages[1].SetPrevCentroidValues(dropletImages[0].GetXCentroid(), dropletImages[0].GetYCentroid());
                dropletImages[0].DetermineVelocity();
                dropletImages[0].DetermineAcceleration();
                dropletImages[0].DetermineVolume();
                /*
                dropletImages[1].SetPrevVelocityValues(dropletImages[0].GetXVelocity(), dropletImages[0].GetYVelocity());
                dropletImages[1].DetermineCentroid();
                dropletImages[1].DetermineVelocity();
                dropletImages[1].DetermineAcceleration();
                dropletImages[1].DetermineVolume();
                 */

                //Set displayed image to the fourth in the list and adjust according to new calibration value
                //displayedImage = dropletImages[0].GetBlackWhiteImage();
                //displayedImage = dropletImages[0].GetConvergence();
                displayedImage = dropletImages[0].GetDropImage();
                
                //Set picturebox to black and white image
                currentImagePictureBox.Image = displayedImage;

                //Enable the 'Run' button and 'Calibrate' button
                runButton.Enabled = true;
                runToolStripMenuItem.Enabled = true;
                calibrateButton.Enabled = true;
                
            }
            else
            {
                MessageBox.Show("Image folder was not selected.", "Loading Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            //loadingWindow.Hide();
        }

        private void baseNeedleHeightTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void blackWhiteNumericUpDown_ValueChanged(object sender, EventArgs e)
        {

        }

        //Note: Calculations change everytime you calibrate... for some reason
        private void calibrateButton_Click(object sender, EventArgs e)
        {

            Graphics graphic = Graphics.FromImage(currentImagePictureBox.Image);
            graphic.Clear(Color.White);//Color to fill the background and reset the box

            /* Create convergence matrix containing location of just needle and base */
            CreateConvergenceMatrix();

            /* Use distance between base and needle in pixels 
               and baseNeedleHeight in cm to calculate cm per pixel */
            ConvertPxToCm();

            dropletImages[0].PreprocessImage();
            dropletImages[0].DetermineCentroid();
            dropletImages[0].DetermineVelocity();
            dropletImages[0].DetermineAcceleration();
            dropletImages[0].DetermineVolume();

            //Set displayed image to the fourth in the list and adjust according to new calibration value
            //displayedImage = dropletImages[0].GetBlackWhiteImage();
            //displayedImage = dropletImages[0].GetConvergence();
            displayedImage = dropletImages[0].GetDropImage();
            
            //Set picturebox to black and white image
            //currentImagePictureBox.Image = null;
            currentImagePictureBox.Image = displayedImage;
            //currentImagePictureBox.Refresh();


        }

        private void CreateConvergenceMatrix()
        {

            int greyscaleThreshold = (int)blackWhiteNumericUpDown.Value;
            //Set the greyscale threshold
            DropletImage.SetGreyScaleThreshold(greyscaleThreshold);

            //Initialize convergence matrix to be all true.
            dropletImages[0].InitializeConvergenceMatrix();
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
        }

        private void ConvertPxToCm()
        {
            //Convert pixels to cm units using baseNeedleHeight relationship
            // -based on convergence matrix
            if (baseNeedleHeightTextBox.Text != "")
            {
                baseToNeedleHeight = Double.Parse(baseNeedleHeightTextBox.Text);
            }
            DropletImage.ConvertPixelToMicron(baseToNeedleHeight);
        }

        private void runButton_Click(object sender, EventArgs e)
        {
            //Begin execution time timer
            int startTime = System.Environment.TickCount;

            //statusLabel.Text = "Processing...";
            frameRate = (int)frameRateNumericUpDown.Value;
            DropletImage.ConvertFRtoSecPerImage(frameRate);

            //Preprocess each image
            string labelValue = "Preprocessing Images: ";
            statusLabel.Text = labelValue;
            backgroundWorker.RunWorkerAsync();
            /*Application.DoEvents();
            int currentProgress = 0;
            runProgressBar.Maximum = dropletImages.Length;
            Parallel.ForEach(dropletImages, dropletImage =>
            {
                dropletImage.PreprocessImage();

                //Update status label and progress bar
                //statusLabel.Text = labelValue + (i + 1).ToString();
                currentProgress++;
                //runProgressBar.Value = currentProgress;
            });
            */

            //Determine centroids of each image
            labelValue = "Determining Centroids: ";
            statusLabel.Text = labelValue;
            Application.DoEvents();
            runProgressBar.Maximum = dropletImages.Length;
            //currentProgress = 0;
            Parallel.ForEach(dropletImages, dropletImage =>
            {
                dropletImage.DetermineCentroid();

                //Update status label and progress bar
                //statusLabel.Text = labelValue + (i + 1).ToString();
                //currentProgress++;
                //runProgressBar.Value = currentProgress;
            });

            //Pass the previous image's centroid to each image
            for (int i = 1; i < dropletImages.Length; i++)
            {
                dropletImages[i].SetPrevCentroidValues(dropletImages[i - 1].GetXCentroid(), dropletImages[i - 1].GetYCentroid());
            }

            //Determine velocity of each image
            labelValue = "Determining Velocities: ";
            statusLabel.Text = labelValue;
            Application.DoEvents();
            Parallel.ForEach(dropletImages, dropletImage =>
            {
                dropletImage.DetermineVelocity();

                //Update status label and progress bar
                //statusLabel.Text = labelValue + (i + 1).ToString();
                //currentProgress++;
                //runProgressBar.Value = currentProgress;
            });

            //Pass the previous image's velocity to each image
            for (int i = 1; i < dropletImages.Length; i++)
            {
                dropletImages[i].SetPrevVelocityValues(dropletImages[i - 1].GetXVelocity(), dropletImages[i - 1].GetYVelocity());
            }

            //Determine acceleration of each image
            labelValue = "Determining Accelerations: ";
            statusLabel.Text = labelValue;
            Application.DoEvents();
            Parallel.ForEach(dropletImages, dropletImage =>
            {
                dropletImage.DetermineAcceleration();

                //Update status label and progress bar
                //statusLabel.Text = labelValue + (i + 1).ToString();
                //currentProgress++;
                //runProgressBar.Value = currentProgress;
            });

            //Determine volume of each image
            labelValue = "Determining Volumes: ";
            statusLabel.Text = labelValue;
            Application.DoEvents();
            Parallel.ForEach(dropletImages, dropletImage =>
            {
                dropletImage.DetermineVolume();

                //Update status label and progress bar
                //statusLabel.Text = labelValue + (i + 1).ToString();
                //currentProgress++;
                //runProgressBar.Value = currentProgress;
            });

            //Create Output object to create Excel file
            Output output = new Output(folderPath, dropletImages.Length);
            statusLabel.Text = "Creating Excel Graphs";
            Application.DoEvents();

            //Pass information into output
            for (int i = 0; i < dropletImages.Length; i++)
            {
                output.insertRow(dropletImages[i], i);
            }

            //End execution timer
            int stopTime = System.Environment.TickCount;
            Console.WriteLine("Time: " + (stopTime - startTime));

            //Create Excel file
            output.generateExcel();
            statusLabel.Text = "Processing Complete!";
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

        private void browseButton_Click(object sender, EventArgs e)
        {
            saveFileDialog.FileName = "output.xlsx";
            saveFileDialog.Filter = "Excel Files (*.xlsx)|*.xlsx";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                folderPath = saveFileDialog.FileName;
                saveDestinationTextBox.Text = folderPath;
            }
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            int currentProgress = 0;
            Parallel.ForEach(dropletImages, dropletImage =>
            {
                dropletImage.PreprocessImage();

                //Update progress bar
                currentProgress++;
                backgroundWorker.ReportProgress(currentProgress);
            });
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            runProgressBar.Value = e.ProgressPercentage;
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            statusLabel.Text = "Generating output...";
            runProgressBar.Value = runProgressBar.Maximum;
        }
    }
}
