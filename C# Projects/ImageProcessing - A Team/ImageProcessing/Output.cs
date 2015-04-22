using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using System.Diagnostics;

namespace ImageProcessing
{
    class Output
    {
        string fileName;
        DropletImage[] dropletImages;
        const int numOfScatterPlotGraphs = 9;
        Excel.Workbook xlWB;
        Excel._Worksheet xlWSData;

        public Output(string inputFileName, int numImages)
        {
            fileName = inputFileName;
            dropletImages = new DropletImage[numImages];
        }

        public void insertRow(DropletImage input, int index)
        {
            dropletImages[index] = input;
        }

        public void generateExcel()
        {
            //check if Excel Application is already opened.. if so kill Excel process
            Process[] processlist = Process.GetProcesses();
            foreach (Process process in processlist)
            {
                if (process.ProcessName == "Excel")
                {
                    Console.WriteLine("find execl");
                    process.Kill();
                }
            }

            //Create Excel Application
            var xlApp = new Excel.Application();

            //If Excel Application is not installed on machine
            if (xlApp == null)
            {
                Console.WriteLine("Excel could not be started. Check that your office installation is correct");
            }
            else
            {
                //Allows Excel Application to be written to/read from
                xlApp.Visible = true;

                //Create workbook for Excel Application
                xlWB = xlApp.Workbooks.Add(Excel.XlWBATemplate.xlWBATWorksheet);

                //Create worksheet for each graph 
                for (int i = 0; i < numOfScatterPlotGraphs; i++)
                {
                    xlWB.Worksheets.Add();
                }

                //Create processed data worksheet within Workbook
                Excel._Worksheet xlWSData = (Excel._Worksheet)xlWB.Worksheets.get_Item(1);
                xlWSData.Name = "Processed Data";

                //Create header for each workSheet within Excel Workbook
                xlWSData.Cells[1, "A"] = "";
                xlWSData.Cells[1, "B"] = "Time";
                xlWSData.Cells[1, "C"] = "X Centroid";
                xlWSData.Cells[1, "D"] = "Y Centroid";
                xlWSData.Cells[1, "E"] = "X Velocity";
                xlWSData.Cells[1, "F"] = "Y Velocity";
                xlWSData.Cells[1, "G"] = "Net Velocity";
                xlWSData.Cells[1, "H"] = "X Acceleration";
                xlWSData.Cells[1, "I"] = "Y Acceleration";
                xlWSData.Cells[1, "J"] = "Net Acceleration";
                xlWSData.Cells[1, "K"] = "Volume";

                //Bold the header in the data worksheet 
                Excel.Range formatRange;
                formatRange = xlWSData.get_Range("A1");
                formatRange.EntireRow.Font.Bold = true;

                //Autofit each cell in data worksheet based on data
                formatRange = xlWSData.get_Range("A:I");
                formatRange.Columns.AutoFit();

                for (int i = 0; i < dropletImages.Length; i++)
                {
                    //loop through each processed image here creating excel file...
                    xlWSData.Cells[i + 2, "A"] = i + 1;
                    xlWSData.Cells[i + 2, "B"] = dropletImages[i].GetTime();
                    xlWSData.Cells[i + 2, "C"] = dropletImages[i].GetXCentroid();
                    xlWSData.Cells[i + 2, "D"] = dropletImages[i].GetYCentroid();
                    xlWSData.Cells[i + 2, "E"] = dropletImages[i].GetXVelocity();
                    xlWSData.Cells[i + 2, "F"] = dropletImages[i].GetYVelocity();
                    xlWSData.Cells[i + 2, "G"] = dropletImages[i].GetNetVelocity();
                    xlWSData.Cells[i + 2, "H"] = dropletImages[i].GetXAcceleration();
                    xlWSData.Cells[i + 2, "I"] = dropletImages[i].GetYAcceleration();
                    xlWSData.Cells[i + 2, "J"] = dropletImages[i].GetNetAcceleration();
                    xlWSData.Cells[i + 2, "K"] = dropletImages[i].GetVolume();
                }
                //Creation of Scatterplots 

                //X Centroid
                CreateScatterPlotGraph(2, "X Centroid", "C", xlWSData);
                //Y Centroid
                CreateScatterPlotGraph(3, "Y Centroid", "D", xlWSData);
                //X Velocity
                CreateScatterPlotGraph(4, "X Velocity", "E", xlWSData);
                //Y Velocity
                CreateScatterPlotGraph(5, "Y Velocity", "F", xlWSData);
                //Net Velocity
                CreateScatterPlotGraph(6, "Net Velocity", "G", xlWSData);
                //X Acceleration
                CreateScatterPlotGraph(7, "X Acceleration", "H", xlWSData);
                //Y Acceleration
                CreateScatterPlotGraph(8, "Y Acceleration", "I", xlWSData);
                //Net Acceleration
                CreateScatterPlotGraph(9, "Net Acceleration", "J", xlWSData);
                //Volume
                CreateScatterPlotGraph(10, "Volume", "K", xlWSData);

                xlWB.SaveAs(fileName);

                if (Directory.Exists(fileName))
                {
                    xlApp.Workbooks.Open(fileName);
                    xlWSData.Activate();
                }

            }
        }

        //@param int        graphNum            gets worksheet number
        //@param string     scatterPlotNmae     gets name of scatterplot
        //@param string     dataColumn          gets the column for each set of data (velocity, acceleraton...)
        private void CreateScatterPlotGraph(int graphNum, string scatterPlotName, string dataColumn, Excel._Worksheet xlWSData)
        {
            Excel._Worksheet XlWSScattPlot = (Excel._Worksheet)xlWB.Worksheets.get_Item(graphNum);
            XlWSScattPlot.Name = scatterPlotName;

            Excel.ChartObjects chartObjs = (Excel.ChartObjects)XlWSScattPlot.ChartObjects(Type.Missing);

            //Add method (left, top, width, height) probably need to mess around with the width and height of the graph
            Excel.ChartObject chartObj = chartObjs.Add(0, 0, 500, 500);
            Excel.Chart xlChart = chartObj.Chart;

            //Scatterplot graph is created based on processed data worksheet, B:B is the time column 
            Excel.Range chartRange = xlWSData.get_Range("B:B," + dataColumn + ":" + dataColumn);
            xlChart.SetSourceData(chartRange, Type.Missing);
            xlChart.ChartType = Excel.XlChartType.xlXYScatter;

            //Customize axis
            Excel.Axis xAxis = (Excel.Axis)xlChart.Axes(Excel.XlAxisType.xlCategory, Excel.XlAxisGroup.xlPrimary);
            xAxis.HasMajorGridlines = true;
            xAxis.HasTitle = true;
            xAxis.AxisTitle.Text = "Time";

            Excel.Axis yAxis = (Excel.Axis)xlChart.Axes(Excel.XlAxisType.xlValue, Excel.XlAxisGroup.xlPrimary);
            yAxis.MajorTickMark = Excel.XlTickMark.xlTickMarkCross;
            yAxis.HasTitle = true;
            yAxis.AxisTitle.Text = scatterPlotName;

            xlChart.HasTitle = true;
            xlChart.ChartTitle.Text = scatterPlotName + "/Time";
        }


        /*
        //May need to release COM object
        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Exception Occured while releasing object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }
         * */
    }
}
