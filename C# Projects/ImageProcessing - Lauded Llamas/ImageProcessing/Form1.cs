/*      Motion Droplet Image Processing
 * 
 *  The following program is designed by the Lauded Llamas Software Engineering 
 *  group of MWSU. The main objective of the image processing is to locate the 
 *  centroid of the droplet in each frame in terms of x and y coordinates and 
 *  additionally calculate the velocity and acceleration of the droplet in each image. 
 *  Test images and a .csv file will be created for data collection. Please refer 
 *  to the User Manual for a guided walktrough of the program.
 */

//Form 1

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ImageProcessing
{
    public partial class Form1 : Form
    {
        Bitmap baseimage;                       //Base Image
        Bitmap bwbaseimage;                     //Base Image for black and white conversion
        int graysensitivity = 32;               //Grayscaled sensitivity.     Black <= 32   White > 32; changes w/ slider and updown                   
        int maxdropletarea, mindropletarea;     //Area/Space between the bottom of the needle and the top of the base
        bool[,] boolbasearray;                  //Array for removing the base, True if part of the base, False if not
        bool allblackrow, allwhiterow;          //Used to determine the max and min range/area 
        bool firstblackrow, firstwhiterow;      //..
        Form2 form2;                            //Handle for Form 2

        public Form1()
        {
            InitializeComponent();
        }

        /*
                Button - Open Base Image
          
         * Opens file dialog window
         * 
         * Sets baseimage and bwbaseimage based on selected filepath
         * and displays the image in pictureBox1 after TestButton has been selected
         */
        private void button1_Click(object sender, EventArgs e)
        {
             DialogResult dr = openFileDialog1.ShowDialog();

            try
            {
                if (dr == DialogResult.OK)
                {
                    baseimage = new Bitmap(openFileDialog1.FileName);
                    bwbaseimage = new Bitmap(baseimage);
                    pictureBox1.Image = baseimage;
                    button3.Enabled = true; // enable Test button
                }
            }
            catch
            {
                MessageBox.Show("Please select a valid image");
            }
        }

        /*
                PictureBox1 - Displays Base Image
          
         *  displays after open base image is loaded 
         */
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        /*
                TrackBar - Gray sensitivity  
         
         *  Displays the grayscale sensitivity to be applied for Test button.
         *  TrackBar and Numeric Up and Down both display the same info in a different manner.
         */
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            graysensitivity = Convert.ToInt32(trackBar1.Value);
            numericUpDown1.Value = graysensitivity;
            trackBar1.Value = graysensitivity;

            button2.Enabled = false; // disable Accept button
        }

        /*
                Numeric Up and Down - Gray sensitivity  
         
         *  Displays the grayscale sensitivity to be applied for Test button.
         *  Numeric Up and Down and TrackBar both display the same info in a different manner.
         */
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            graysensitivity = Convert.ToInt32(numericUpDown1.Value);
            numericUpDown1.Value = graysensitivity;
            trackBar1.Value = graysensitivity;

            button2.Enabled = false; // disable Accept button
        }

        /*
                Button - Test
          
         *  This is the main function of form1, intended to remove the needle and base from the image.
         *  Does so by looking at each pixel in the base image and determining if the grayscaled value
         *  of the the pixel is part of the needle/base or part of the background. It also scans a row in the x-direction 
         *  looking for the first all white row of pixels and sets that as the "maxdropletarea", then continues to
         *  find the first all black row of pixels and sets that as the "mindropletarea". Our area of intrest is
         *  narrowed down by these values and also by the boolean array of values for the needle and base.
         */
        private void button3_Click(object sender, EventArgs e)
        {

            boolbasearray = new bool[baseimage.Width+1, baseimage.Height+1];    //Initialize
            firstblackrow = true;                                               //
            firstwhiterow = true;                                               //

            for (int y = 0; y < baseimage.Height; y++)                          //rows (ypos) in bitmap
            {
                allwhiterow = true;                                             //
                allblackrow = true;                                             //

                for (int x = 0; x < baseimage.Width; x++)                       //columuns (xpos) in bitmap
                {
                    
                    Color originalcolor = baseimage.GetPixel(x, y);             //Grayscaling the pixel in question
                    int grayscale = (int)((originalcolor.R * .3) + (originalcolor.G * .59) + (originalcolor.B * .11));
                    Color tempgrayscale = Color.FromArgb(grayscale, grayscale, grayscale);


                    if (grayscale <= graysensitivity)                           //If pixel in question is <=  graysensitivity(value is set by slider)...
                    {
                        bwbaseimage.SetPixel(x, y, Color.Black);                //..Color pixel black..
                        boolbasearray[x, y] = true;                             //..set to true..
                        allwhiterow = false;                                    //..and cant be an all whiterow

                        if (allblackrow == true && x == baseimage.Width-1 && firstblackrow == true) //if last pixel in row && this happens
                        {
                            mindropletarea = y;                                             // found min droplet area
                            firstblackrow = false;
                            for (int min = 0; min < baseimage.Width; min++)                 //draw line to display
                            {
                                bwbaseimage.SetPixel(min, mindropletarea, Color.Cyan);

                            }
                        }
                    }

                    else                                                         //If pixel in question is > graysensitivity(value is set by slider)...
                    {
                        bwbaseimage.SetPixel(x, y, Color.White);                //..Color pixel white..
                        boolbasearray[x, y] = false;                            //..set to false..
                        allblackrow = false;                                    //..and cant be an all blackrow

                        if (allwhiterow == true && x == baseimage.Width - 1 && firstwhiterow == true)   //if last pixel in row && this happens
                        {
                            maxdropletarea = y;                                             // found max droplet area
                            firstwhiterow = false;
                            for (int max = 0; max < baseimage.Width; max++)                 //draw line to display
                            {
                                bwbaseimage.SetPixel(max, maxdropletarea, Color.Cyan);
                            }
                        }
                    }
                }
            }   
            pictureBox2.Image = bwbaseimage;    
            button2.Enabled = true;                                             // enable Accept button
        }

        /*
                PictureBox2 - Displays Base Image
          
         *  displays after TestButton is selected 
         */
        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        /*
                Button - Accept 
          
         *  Transition to form2
         *  
         *  sends over all necessary info to be used in form2
         */
        private void button2_Click(object sender, EventArgs e)
        {
            form2 = new Form2(graysensitivity, boolbasearray, mindropletarea, maxdropletarea);
            form2.Show();
            this.Hide();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
