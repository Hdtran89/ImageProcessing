using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessing
{
    class Output
    {
        string fileName;
		DropletImage[] dropletImages;
		
		public Output(string inputFileName, int numImages){
			fileName = inputFileName;
			dropletImages = new DropletImage[numImages];
		}
		
		public void insertRow(DropletImage input, int index){
			dropletImages[index] = input;
		}

        public void generateExcel()
        {

        }
    }
}
