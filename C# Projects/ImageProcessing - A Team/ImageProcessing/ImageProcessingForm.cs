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
using Excel = Microsoft.Office.Interop.Excel;

namespace ImageProcessing
{

    public partial class ImageProcessingForm : Form
    {
        string[] images;                //Stores file names of all images
        DropletImage[] dropletImages;   //Stores every DropletImage object

        Bitmap displayedImage;
        int frameRate;
        double baseToNeedleHeight = -1; //cm
        string saveDirectoryPath;
        string loadDirectoryPath;
        AboutWindow loadingWindow = new AboutWindow();

        //Run button locks - does not enable until both are true
        bool loadedImages;
        bool setSaveDestination;

        public ImageProcessingForm()
        {
            InitializeComponent();
            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.WorkerSupportsCancellation = true;

            //Initialize Run button locks
            loadedImages = false;
            setSaveDestination = false;
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
                runProgressBar.Maximum = images.Length;

                if (images.Length != 0)
                {
                    //Save the loaded image source directory path
                    loadDirectoryPath = loadImagesDialog.SelectedPath;

                    //Display loading status
                    statusLabel.Text = "Loading data...";
                    //Create a Droplet Image object for every given image
                    dropletImages = new DropletImage[images.Length];
                    string fileName = "";

                    for (int i = 0; i < images.Length; i++)
                    {
                        fileName = new DirectoryInfo(@images[i]).Name;
                        //Create the Droplet Image object
                        dropletImages[i] = new DropletImage(new Bitmap(images[i]), i, fileName);
                    }

                    //Convert framesPerSec to seconds per image
                    DropletImage.ConvertFRtoSecPerImage(frameRate);

                    /* Create convergence matrix containing location of just needle and base */
                    /* Based on Black/White Calibration value */
                    CreateConvergenceMatrix();

                    /* Use distance between base and needle in pixels 
                       and baseNeedleHeight in cm to calculate cm per pixel */
                    DropletImage.ConvertPixelToMicron(baseToNeedleHeight);

                    //Display the number of files loaded in the status label
                    statusLabel.Text = "Loaded " + images.Length + " images.";

                    dropletImages[4].PreprocessImage();
                    dropletImages[4].DetermineCentroid();
                    displayedImage = dropletImages[4].GetColorImage();

                    //Set picturebox to black and white image
                    currentImagePictureBox.Image = displayedImage;

                    //Enable the 'Calibrate' button and specify that images have been loaded
                    loadedImages = true;
                    enableRunButton();
                    calibrateButton.Enabled = true;
                    imagesSourceTextBox.Text = loadDirectoryPath;
                }
                else
                {
                    MessageBox.Show("Image folder was not selected.", "Loading Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
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
            DropletImage.ConvertPixelToMicron(baseToNeedleHeight);

            dropletImages[4].PreprocessImage();
            dropletImages[4].DetermineCentroid();

            //Set displayed image to the fourth in the list and adjust according to new calibration value
            displayedImage = dropletImages[4].GetColorImage();
            
            //Set picturebox to black and white image
            //currentImagePictureBox.Image = null;
            currentImagePictureBox.Image = displayedImage;
            //currentImagePictureBox.Refresh();


        }

        private void disableFormButtons()
        {
            loadButton.Enabled = false;
            browseButton.Enabled = false;
            calibrateButton.Enabled = false;
            runToolStripMenuItem.Enabled = false;
            loadToolStripMenuItem.Enabled = false;
        }

        private void enableFormButtons()
        {
            loadButton.Enabled = true;
            browseButton.Enabled = true;
            calibrateButton.Enabled = true;
            loadToolStripMenuItem.Enabled = true;
        }

        //Validate, handle baseNeedleHeight input and update baseToNeedleHeight if accepted
        private bool validateBaseNeedleHeight()
        {
            try
            {
                if (baseNeedleHeightTextBox.Text != "")
                {
                    if (Double.Parse(baseNeedleHeightTextBox.Text) > 0)
                        baseToNeedleHeight = Double.Parse(baseNeedleHeightTextBox.Text);
                    else
                    {
                        MessageBox.Show("Please enter a positive number for the base/needle height.",
                            "Negative Base/Needle Height", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
                else
                {
                    baseToNeedleHeight = -1;
                }
            }
            catch
            {
                MessageBox.Show("Invalid height input. Please make sure you input a positive numeric value.",
                    "Invalid Base/Needle Height", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
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

       

        private void runButton_Click(object sender, EventArgs e)
        {
            //Validate base/needle height
            if (validateBaseNeedleHeight() == false)
            {
                return;
            }

            //Disable the form buttons
            disableFormButtons();
            runToolStripMenuItem.Enabled = false;
            
            //Necessary operations before processing begins
            CreateConvergenceMatrix();
            DropletImage.ConvertPixelToMicron(baseToNeedleHeight);

            frameRate = (int)frameRateNumericUpDown.Value;
            DropletImage.ConvertFRtoSecPerImage(frameRate);

            //Begin Image Processing
            backgroundWorker.RunWorkerAsync();

            //Change Run button into Stop button
            runButton.Text = "Stop";
            runButton.Click -= this.runButton_Click;
            runButton.Click += this.stopButton_Click;
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            backgroundWorker.CancelAsync();
        }
		
        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frameRateNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            frameRate = (int)frameRateNumericUpDown.Value;
            //Console.WriteLine(frameRate);
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            saveFileDialog.FileName = "output.xlsx";
            saveFileDialog.Filter = "Excel Files (*.xlsx)|*.xlsx";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                saveDirectoryPath = saveFileDialog.FileName;
                saveDestinationTextBox.Text = saveDirectoryPath;

                //Specify that the save destination has been set
                setSaveDestination = true;
                enableRunButton();
            }
        }
        /** Creates new files for the processed images
         *  or overrides file if they already exist
         */
        private void CreateProcessedImageFile(string path)
        {
            if (!File.Exists(path))
            {
                File.Create(path).Close();
            }
            else
            {
                File.Delete(path);
                File.Create(path).Close();
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

                //Check if user cancelled processing
                if (backgroundWorker.CancellationPending)
                {
                    e.Cancel = true;
                    backgroundWorker.ReportProgress(0);
                    return;
                }
            });

            //Determine centroid of each image
            backgroundWorker.ReportProgress(-1);
            Parallel.ForEach(dropletImages, dropletImage =>
            {
                dropletImage.DetermineCentroid();
            });

            //Pass the previous image's centroid to each image
            for (int i = 1; i < dropletImages.Length; i++)
            {
                dropletImages[i].SetPrevCentroidValues(dropletImages[i - 1].GetXCentroid(), dropletImages[i - 1].GetYCentroid());
            }

            //Check if user cancelled processing
            if (backgroundWorker.CancellationPending)
            {
                e.Cancel = true;
                backgroundWorker.ReportProgress(0);
                return;
            }

            //Determine velocity of each image
            Parallel.ForEach(dropletImages, dropletImage =>
            {
                dropletImage.DetermineVelocity();
            });

            //Pass the previous image's velocity to each image
            for (int i = 1; i < dropletImages.Length; i++)
            {
                dropletImages[i].SetPrevVelocityValues(dropletImages[i - 1].GetXVelocity(), dropletImages[i - 1].GetYVelocity());
            }

            //Check if user cancelled processing
            if (backgroundWorker.CancellationPending)
            {
                e.Cancel = true;
                backgroundWorker.ReportProgress(0);
                return;
            }

            //Determine acceleration of each image
            Parallel.ForEach(dropletImages, dropletImage =>
            {
                dropletImage.DetermineAcceleration();
            });

            //Determine volume of each image
            Parallel.ForEach(dropletImages, dropletImage =>
            {
                dropletImage.DetermineVolume();
            });

            //Check if user cancelled processing
            if (backgroundWorker.CancellationPending)
            {
                e.Cancel = true;
                backgroundWorker.ReportProgress(0);
                return;
            }

            //Create Output object to create Excel file
            backgroundWorker.ReportProgress(-2);
            Output output = new Output(saveDirectoryPath, dropletImages.Length);

            //Pass information into output
            for (int i = 0; i < dropletImages.Length; i++)
            {
                output.insertRow(dropletImages[i], i);
            }

            //Check if user cancelled processing
            if (backgroundWorker.CancellationPending)
            {
                e.Cancel = true;
                backgroundWorker.ReportProgress(0);
                return;
            }

            //Create Excel file
            output.generateExcel();

            //Check if user cancelled processing
            if (backgroundWorker.CancellationPending)
            {
                e.Cancel = true;
                backgroundWorker.ReportProgress(0);
                return;
            }

            //Create new folder for the processed images to be created
            string dirName = Path.GetDirectoryName(loadDirectoryPath);
            FileInfo fInfo = new FileInfo(images[0]);
            string imageFolderName = fInfo.Directory.Name;
            string newDirectory = dirName + "/" + imageFolderName + "_processed";
            if (!Directory.Exists(newDirectory))
                Directory.CreateDirectory(newDirectory);

            //Save every processed image
            Parallel.ForEach(dropletImages, dropletImage =>
            {
                string newImageFile = newDirectory + "/" + dropletImage.GetImageName();
                CreateProcessedImageFile(newImageFile);
                //Save the processed image to newImageFile
                dropletImage.GetBlackWhiteImage().Save(newImageFile);

                //Update the UI with the current progress
                currentProgress++;
                backgroundWorker.ReportProgress(currentProgress);

                //Check if user cancelled processing
                if (backgroundWorker.CancellationPending)
                {
                    e.Cancel = true;
                    backgroundWorker.ReportProgress(0);
                    return;
                }
            });
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage > 0 && e.ProgressPercentage <= dropletImages.Length)
            {
                statusLabel.Text = "Processing Images: " + e.ProgressPercentage.ToString() + "/" + dropletImages.Length.ToString();
                runProgressBar.Value = e.ProgressPercentage;
            }
            if (e.ProgressPercentage > dropletImages.Length)
            {
                int currentProgress = e.ProgressPercentage - dropletImages.Length;
                statusLabel.Text = "Saving Processed Images: " + currentProgress.ToString() + "/" + dropletImages.Length.ToString();
                runProgressBar.Value = currentProgress;
            }
            if (e.ProgressPercentage == -1)
            {
                statusLabel.Text = "Calculating Drop Measurements";
            }
            if (e.ProgressPercentage == -2)
            {
                statusLabel.Text = "Generating Spreadsheet/Data Plots";
            }
            if (e.ProgressPercentage == 0)
            {
                statusLabel.Text = "Stopped Processing";
                runProgressBar.Value = 0;

                //Change Stop button back into Run button
                runButton.Text = "Run";
                runButton.Click -= this.stopButton_Click;
                runButton.Click += this.runButton_Click;

                //Enable the various form buttons
                enableFormButtons();
            }
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            statusLabel.Text = "Processing Complete!";
            runProgressBar.Value = runProgressBar.Maximum;

            //Reset the save destination
            saveDirectoryPath = "";
            saveDestinationTextBox.Text = saveDirectoryPath;
            setSaveDestination = false;
            runButton.Enabled = false;

            //Change Stop button back into Run button
            runButton.Text = "Run";
            runButton.Click -= this.stopButton_Click;
            runButton.Click += this.runButton_Click;

            //Enable the various form buttons
            enableFormButtons();
        }

        private void enableRunButton()
        {
            //If both conditions are met, enable the Run button
            if (loadedImages && setSaveDestination)
            {
                runButton.Enabled = true;
                runToolStripMenuItem.Enabled = true;
            }
        }

        private void aboutUsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutWindow aboutWindow = new AboutWindow();
            aboutWindow.ShowDialog();
        }

        
    }
}