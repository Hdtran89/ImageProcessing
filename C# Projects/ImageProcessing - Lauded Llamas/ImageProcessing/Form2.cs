//Form 2

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;


namespace ImageProcessing
{
    public partial class Form2 : Form
    {
        string dropletfoldername;               //Path that contains the droplet images
        Bitmap selectedbitmap;                  //Current image that is being processed
        int grayscalevalue;                     //Grayscaled value from Form1    
        int maxdropletarea, mindropletarea;     //Max and Min area from Form1
        bool[,] boolbasearray;                  //Removal array from Form1
        bool firstXblackpix = true;             //First X black pixel
        int lastXblackpix;                      //Last X black pixel
        bool firstYblackpix = true;             //First Y black pixel
        int lastYblackpix;                      //Last Y black pixel
        int index;                              //Indexed position to insert "bw" to test image
        int boundingrange = 20;                 //From NumericUpandDown, sets the second centroid pass to be within range
        float xcentroid = 0, ycentroid = 0;     //Centroid values
        float time = 0;                         //Current time, 0 - lastframe
        float timeinc = 0;                      //Value set from NumericUpandDown, change between each frame
        float prevtime = 0;                     //Previous time
        float xvelocity = 0;                    //Velocity
        float yvelocity = 0;                    //
        float xaccel = 0;                       //Acceleration
        float yaccel = 0;                       //
        float prevxpos = 0;                     //Previous centroid pos
        float prevypos = 0;                     //
        float prevxvel = 0;                     //Previous velocity
        float prevyvel = 0;                     //
        List<Coord> OuterPixels;                //OuterPixels of droplet
        List<Coord> Outliers;                   //Outliers from droplet centriod, based on boundingrange
        Form1 form1;                            //return to form1 after processing


        public Form2()
        {
            InitializeComponent();
        }

        /*
                Form2 Initialization from form1
         *      
         *  Values are passed from form1 here. 
         *  Intialization of form2 as well.
         */
        public Form2(int gray, bool[,] array, int min, int max)
        {
            grayscalevalue = gray;      //Set values that are recieved from form1
            boolbasearray = array;      
            mindropletarea = min;       
            maxdropletarea = max;       
            InitializeComponent();      //Init form2
        }

        /*
                Coord Struct
          
         *  Struct for OuterPixel and Outliers coordinates.
         *  Allows for the list to have an x and y value.   
         */
        public struct Coord
        {
            public int outerx;
            public int outery;
        }

        /*
                Open Droplet Images Folder
         
         *  Read in the droplet images from the selected folder.
         *  The selected folder will look for files with the same type as the radio button(tif,bmp,...etc).
         *  The images will be stored and displayed in the ListBox.
         */
        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult dr = folderBrowserDialog1.ShowDialog();               // Open folder dialog browser

            if (dr == DialogResult.OK)
            {
                dropletfoldername = folderBrowserDialog1.SelectedPath;          //Select the folder name

                if (radioButton1.Checked)                                       //For .tif files                                   
                {
                    foreach (string path in Directory.GetFiles(dropletfoldername, "*.tif"))  //If you want to change the filetype that the browser is looking for, do so here 
                    {                                                                       
                        if(!path.Contains("bw"))                                //dont look for test images that may already be in there                                              
                            listBox1.Items.Add(path);                           //add the image paths to the listbox                           
                        else
                            File.Delete(path);
                    }
                    button3.Enabled = true;                                     //Enable Runbutton
                }
                else if (radioButton2.Checked)                                  //For .bmp files
                {
                    foreach (string path in Directory.GetFiles(dropletfoldername, "*.bmp"))  //If you want to change the filetype that the browser is looking for, do so here
                    {
                        if (!path.Contains("bw"))                               //dont look for test images that may already be in there   
                            listBox1.Items.Add(path);                           //add the image paths to the listbox   
                        else
                            File.Delete(path);
                    }
                    button3.Enabled = true;                                     //Enable Runbutton
                }
            }
        }

        /*      
                ListBox
         
         *  Displays the paths of the selected images.
         *  Clicking on a path will display its image on the Picturebox on the left of the form
         */
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {        
            string curItem = listBox1.SelectedItem.ToString();
            selectedbitmap = new Bitmap(curItem);
            pictureBox2.Image = selectedbitmap;
        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
        }

        /*
                Button - Run
         
         * This is the main function of form2. It will process the image paths that are in the listbox.
         * Each image will sweep in the x direction and then the y direction adding the first and last 
         * pixels of the droplet to the Outerpixels List. Then it will calculate the centroid for a first 
         * time and then remove any pixels that are to far away from the centroid and then recalculate.
         * The velocity and acceleration calculations follow and then all of the data for that image row 
         * added to the output of the excel data.
         */
        private void button3_Click(object sender, EventArgs e)
        {
            progressBar1.Maximum = listBox1.Items.Count;                    //set progressbar - how many images?
            string csvfilePath = dropletfoldername + "test.csv";            //Excel filepath
            string testimages = dropletfoldername + "\\testimages\\";       //Test images folder path(does not store the test images)
            string delimiter = ",";                                         //Comma seperated value
            StringBuilder sb = new StringBuilder();                         //csv string
            float[][] output;                                               //csv output data
            string[][] header;                                              // csv header
            timeinc = (float)numericUpDown2.Value;                          //from this form's Time Increment upanddown

            CheckDirectory(testimages);                                     //add the test images folder
            //Checkfileopen(csvfilePath);
            Checkfile(csvfilePath);                                         //add the .csv file

            header = new string[][] { new string[] { "Time", "X Centroid", "Z Centroid", "X Velocity", "Z Velocity", "X Acceleration", "Y Acceleration"} };
            sb.AppendLine(string.Join(delimiter, header[0]));               // write excel header

            foreach (string path in listBox1.Items)
            {
                string curBitmappath = path;                    //Initialize image components each time for the selected image
                selectedbitmap = new Bitmap(curBitmappath);     //
                xcentroid = 0;                                  //
                ycentroid = 0;                                  //
                xaccel = 0;                                     //
                yaccel = 0;                                     //
                OuterPixels = new List<Coord>();                //

                XSweep(selectedbitmap);                         //XSweep
                YSweep(selectedbitmap);                         //YSweep                 

                CalcCentroid();                                 //
                Outliers = new List<Coord>();                   // Re-Calc   
                CalcOutliers();                                 //

                xcentroid = 0;
                ycentroid = 0;
                CalcCentroid();                                 //
                CalcVelocity();                                 //  Set data
                CalcAcceleration();                             //

                selectedbitmap.SetPixel((int)xcentroid, selectedbitmap.Height - (int)ycentroid, Color.Green);       //Only for display(can be found in the tested "bw" image)

                CreateTestImage(curBitmappath);                 //Create a "bw" image for 

                output = new float[][]{new float[]{time,xcentroid,ycentroid, xvelocity, yvelocity, xaccel, yaccel}};
                sb.AppendLine(string.Join(delimiter, output[0]));       // Write excel row data

                prevtime = time;                        //  Hold onto previous and incremented values
                time += timeinc;                        //
                prevxpos = xcentroid;                   //
                prevypos = ycentroid;                   //
                prevxvel = xvelocity;                   //
                prevyvel = yvelocity;                   //
                progressBar1.Increment(1);              //  Progressbar increment change
            }

            File.AppendAllText(csvfilePath, sb.ToString());     //Write all .csv data to the file path

            form1 = new Form1();
            form1.Show();
            this.Hide();                                //Reopen form1 
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
        }

        /*
                Button - Remove
         
         * Removes all items in listbox
         */
        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            button3.Enabled = false;
        }

        /*
               numericUpDown - Bounding Range
         
         * range set for second pass of centroid, 
         * if too low, you may not get all of the outerpixels of the droplet
         */
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            boundingrange = Convert.ToInt32(numericUpDown1.Value);
        }

        /*
                setindex
         
         * setindex after the file type
         */
        private int setindex(string current)
        {
            int setindex = 0;

            if (radioButton1.Checked)
                setindex = current.IndexOf(".TIF");

            else if (radioButton2.Checked)
                setindex = current.IndexOf(".bmp");

            return setindex;
        }

        /*
                CheckDirectory
         */
        private bool CheckDirectory(string check)
        {
            if (!Directory.Exists(check))
            {
                Directory.CreateDirectory(check);
                return true;
            }
            else
                return false;
        }

        /*
               CheckFileOpen
         
        *   NOT WORKING CORRECTLY
        *
        private bool Checkfileopen(string check)
        {
            FileInfo fcheck = new FileInfo(check);
            FileStream stream = null;

            try
            {
                stream = fcheck.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                MessageBox.Show("Please close file(s) before continuing");
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }
            return false; 
        }
        */

        /*
               CheckFileOpen
         
         * override file and create a new one
         */
        private bool Checkfile(string check)
        {        
            if (!File.Exists(check))
            {
                File.Create(check).Close();
                return true;
            }
            else
            {
                File.Delete(check);
                File.Create(check).Close();
                return true;
            }
        }

        /*
               XSweep
         
         * Sweep in x direction. Add first and last pixels to OuterPixels array.
         */
        private void XSweep(Bitmap bitmap)
        {
            for (int y = maxdropletarea + 1; y < mindropletarea; y++)   // only look at droplet area from form1
            {
                firstXblackpix = true;                                  //reset each pass
                lastXblackpix = 0;                                      
                for (int x = 0; x < bitmap.Width - 2; x++)              //bad data in last two (see .tif image)           
                {
                    Color originalcolor = bitmap.GetPixel(x, y);        //Grayscaling the pixel in question
                    int grayscale = (int)((originalcolor.R * .3) + (originalcolor.G * .59) + (originalcolor.B * .11));
                    Color tempgrayscale = Color.FromArgb(grayscale, grayscale, grayscale);

                    if (grayscale <= grayscalevalue && boolbasearray[x, y] == false)//If pixel in question is <=  graysensitivity && its not part of the base...
                    {
                        bitmap.SetPixel(x, y, Color.Black);               //..Color pixel black...

                        if (firstXblackpix == true)                     //...(is it the first X?...            
                        {
                            AddDropletPixel(selectedbitmap, x, y);   //... Add pixel to the array..
                            firstXblackpix = false;                     //... cant be first until next pass.)
                        }
                        lastXblackpix = x;                              //set to the current last x value                      
                    }
                    else
                    {
                        bitmap.SetPixel(x, y, Color.White);             //..Color pixel white...

                        if (x == bitmap.Width - 3 && lastXblackpix > 3) //check for last loop
                        {
                            AddDropletPixel(selectedbitmap, lastXblackpix, y);   
                        }
                    }
                }
            }
        }

        /*
              YSweep
         
        * Sweep in y direction. Add first and last pixels to OuterPixels array.
        */
        private void YSweep(Bitmap bitmap)
        {
            for (int x = 0; x < bitmap.Width - 2; x++)           //bad data in last two (see .tif image) 
            {
                firstYblackpix = true;      //reset each pass
                lastYblackpix = 0;
                lastYblackpix = 0;
                for (int y = maxdropletarea + 1; y < mindropletarea; y++)   // only look at droplet area from form1
                {
                    Color originalcolor = bitmap.GetPixel(x, y);             //Grayscaling the pixel in question
                    int grayscale = (int)((originalcolor.R * .3) + (originalcolor.G * .59) + (originalcolor.B * .11));
                    Color tempgrayscale = Color.FromArgb(grayscale, grayscale, grayscale);

                    if (grayscale <= grayscalevalue && boolbasearray[x, y] == false)//If pixel in question is <=  graysensitivity && its not part of the base...
                    {
                        if (firstYblackpix == true)                      //Is it the first y?...  
                        {
                            AddDropletPixel(selectedbitmap, x, y);      //... Add pixel to the array..
                            firstYblackpix = false;                        //... cant be first until next pass.
                        }
                        lastYblackpix = y;                              //set to the current last y value   
                    }
                    else if (y == mindropletarea - 1 && lastYblackpix != 0)// On last pass check
                    {
                        AddDropletPixel(selectedbitmap, x, lastYblackpix);
                    }
                }
            }
        }


        /*
              AddDropletPixel
         
        *   Part of XandYSweep. Adding the coordinate x and y to the OuterPixel list
        */
        private void AddDropletPixel(Bitmap bitmap, int insx, int insy)
        {
            bitmap.SetPixel(insx, insy, Color.Red);     //color for testing
            Coord addPixel;
            addPixel.outerx = insx;
            addPixel.outery = bitmap.Height - insy;

            if(!OuterPixels.Contains(addPixel))         //Only insert if the coord is not already in there
                OuterPixels.Add(addPixel);
        }

        /*
              CalcCentroid  
         
        *   Summation of the x and y values divided by the size of the OuterPixels list
        */
        private void CalcCentroid()
        {
            foreach (Coord outerpixel in OuterPixels)
            {
                xcentroid += outerpixel.outerx;
                ycentroid += outerpixel.outery;
            }
            xcentroid /= OuterPixels.Count;
            ycentroid /= OuterPixels.Count;
        }

        /*
              CalcOutliers 
         
        *   If a pixel is too far from the centoid based on the bounding range, remove that pixel from the OuterPixel list
        */
        private void CalcOutliers()
        {
            foreach (Coord pixel in OuterPixels)
            {
                if (pixel.outerx < xcentroid - boundingrange || pixel.outerx > xcentroid + boundingrange ||
                    pixel.outery > ycentroid + boundingrange || pixel.outery < ycentroid - boundingrange)
                    Outliers.Add(pixel);
            }

            foreach (Coord pixel in Outliers)
            {
                OuterPixels.Remove(pixel);
                selectedbitmap.SetPixel(pixel.outerx, selectedbitmap.Height - pixel.outery, Color.White);
            }
        }

        /*
              CalcVelocity 
         
        *   Change in centroid position over time
        */
        private void CalcVelocity()
        {
            if (time != 0)
            {
                xvelocity = (xcentroid - prevxpos) / (time - prevtime);
                yvelocity = (ycentroid - prevypos) / (time - prevtime);
            }
        }

        /*
              CalcAcceleration
         
        *   Change in velocity over time
        */
        private void CalcAcceleration()
        {
            if (time != 0)
            {
                xaccel = (xvelocity - prevxvel) / (time - prevtime);
                yaccel = (yvelocity - prevyvel) / (time - prevtime);
            }
        }

        /*
              CreateTestImage 
         
        *   Save and create a test image based on the changes made to the current bitmap.  
        */
        private void CreateTestImage(string bitmappath)
        {
            index = setindex(bitmappath);
            string curItem2 = bitmappath.Insert(index, "bw");
            //Checkfileopen(curItem2);
            Checkfile(curItem2);
            selectedbitmap.Save(curItem2);
        }

        /*
             numericUpDown - Time Increment 
         
       *   set the time increment between each frame/image 
       */
        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            timeinc = (float)numericUpDown2.Value;
        }

        /*
            numericUpDown - Width Conversion    
         
        *  width conversion from pixels to cm   (NOT IMPLEMENTED)    
        */
        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            // selectedimage.Width/(cm/mm) 
        }       
    }
}
