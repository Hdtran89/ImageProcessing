using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageProcessing
{
    public partial class AboutWindow : Form
    {
        public AboutWindow()
        {
            InitializeComponent();
            aboutLabel.Text = "Developed by:\n" +
                               "    " + "Sanan Aamir" + "\n" +
                               "    " + "Romando Garcia" + "\n" +
                               "    " + "Anne Lam" + "\n" +
                               "    " + "James Rowe" + "\n" +
                               "    " + "Hieu Tran" + "\n\n" +
                               "Midwestern State University - CMPS 4113" + "\n" +
                               "Professor: Dr. Catherine Stringfellow" + "\n" +
                               "Spring 2015";
        }
    }
}
