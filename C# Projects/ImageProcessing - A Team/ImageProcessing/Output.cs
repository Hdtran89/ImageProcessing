using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

namespace ImageProcessing
{
    class Output
    {
        string fileName;
        DropletImage[] dropletImages;
        const int numOfScatterPlotGraphs = 10;
        Excel.Workbook xlWB;

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
                Console.WriteLine(process.ProcessName);
                if (process.ProcessName == "EXCEL")
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

                Excel._Worksheet tempData = (Excel._Worksheet)xlWB.Worksheets.get_Item(2);
                tempData.Name = "Test Data";
                tempData.Visible = Excel.XlSheetVisibility.xlSheetHidden;
                //create headers for spreadsheets
                ExcelHeader(xlWSData, true);
                ExcelHeader(tempData, false);

                //Bold the header in the data worksheet
                Excel.Range formatRange;
                formatRange = xlWSData.get_Range("A1");
                formatRange.EntireRow.Font.Bold = true;

                //Autofit each cell in data worksheet based on data
                formatRange = xlWSData.get_Range("A:J");
                formatRange.Columns.AutoFit();

                for (int i = 0; i < dropletImages.Length; i++)
                {
                    //loop through each processed image here creating excel file...
                    xlWSData.Cells[i + 2, "A"] = dropletImages[i].GetTime();
                    xlWSData.Cells[i + 2, "B"] = dropletImages[i].GetXCentroid();
                    xlWSData.Cells[i + 2, "C"] = dropletImages[i].GetYCentroid();
                    xlWSData.Cells[i + 2, "D"] = dropletImages[i].GetXVelocity();
                    xlWSData.Cells[i + 2, "E"] = dropletImages[i].GetYVelocity();
                    xlWSData.Cells[i + 2, "F"] = dropletImages[i].GetNetVelocity();
                    xlWSData.Cells[i + 2, "G"] = dropletImages[i].GetXAcceleration();
                    xlWSData.Cells[i + 2, "H"] = dropletImages[i].GetYAcceleration();
                    xlWSData.Cells[i + 2, "I"] = dropletImages[i].GetNetAcceleration();
                    xlWSData.Cells[i + 2, "J"] = dropletImages[i].GetVolume();

                    //excel file created to generate scatter plot graphs
                    tempData.Cells[i + 2, "A"] = dropletImages[i].GetTime();
                    tempData.Cells[i + 2, "B"] = dropletImages[i].GetXCentroid();
                    tempData.Cells[i + 2, "C"] = dropletImages[i].GetYCentroid();
                    tempData.Cells[i + 2, "D"] = dropletImages[i].GetXVelocity();
                    tempData.Cells[i + 2, "E"] = dropletImages[i].GetYVelocity();
                    tempData.Cells[i + 2, "F"] = dropletImages[i].GetNetVelocity();
                    tempData.Cells[i + 2, "G"] = dropletImages[i].GetXAcceleration();
                    tempData.Cells[i + 2, "H"] = dropletImages[i].GetYAcceleration();
                    tempData.Cells[i + 2, "I"] = dropletImages[i].GetNetAcceleration();
                    tempData.Cells[i + 2, "J"] = dropletImages[i].GetVolume();
                }
                //Creation of Scatterplots 

                //X Centroid
                CreateScatterPlotGraph(3, "X Centroid", "B", tempData);
                //Y Centroid
                CreateScatterPlotGraph(4, "Y Centroid", "C", tempData);
                //X Velocity
                CreateScatterPlotGraph(5, "X Velocity", "D", tempData);
                //Y Velocity
                CreateScatterPlotGraph(6, "Y Velocity", "E", tempData);
                //Net Velocity
                CreateScatterPlotGraph(7, "Net Velocity", "F", tempData);
                //X Acceleration
                CreateScatterPlotGraph(8, "X Acceleration", "G", tempData);
                //Y Acceleration
                CreateScatterPlotGraph(9, "Y Acceleration", "H", tempData);
                //Net Acceleration
                CreateScatterPlotGraph(10, "Net Acceleration", "I", tempData);
                //Volume
                CreateScatterPlotGraph(11, "Volume", "J", tempData);
                
                xlWB.SaveAs(fileName);

                if (Directory.Exists(fileName))
                {
                    xlApp.Workbooks.Open(fileName);
                    xlWSData.Activate();

                }

                releaseObject(xlApp);
            }
        }

        //@param int        graphNum            gets worksheet number
        //@param string     scatterPlotNmae     gets name of scatterplot
        //@param string     dataColumn          gets the column for each set of data (velocity, acceleraton...)
        private void CreateScatterPlotGraph(int graphNum, string scatterPlotName, string dataColumn, Excel._Worksheet temp)
        {
            Excel._Worksheet XlWSScattPlot = (Excel._Worksheet)xlWB.Worksheets.get_Item(graphNum);
            XlWSScattPlot.Name = scatterPlotName;

            Excel.ChartObjects chartObjs = (Excel.ChartObjects)XlWSScattPlot.ChartObjects(Type.Missing);

            //Add method (left, top, width, height) probably need to mess around with the width and height of the graph
            Excel.ChartObject chartObj = chartObjs.Add(0, 0, 500, 500);
            Excel.Chart xlChart = chartObj.Chart;
            
            //Scatterplot graph is created based on processed data worksheet, B:B is the time column 
            Excel.Range chartRange = temp.get_Range("A:A," + dataColumn + ":" + dataColumn);
            
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

        private void ExcelHeader(Excel._Worksheet header, bool showTime)
        {
            //Get image units for column headers
            string units = dropletImages[0].GetUnit();
            if (showTime)
            {
                header.Cells[1, "A"] = "Time";
            }
            header.Cells[1, "B"] = "X Centroid (" + units + ")";
            header.Cells[1, "C"] = "Y Centroid (" + units + ")";
            header.Cells[1, "D"] = "X Velocity (" + units + "/s)";
            header.Cells[1, "E"] = "Y Velocity (" + units + "/s)";
            header.Cells[1, "F"] = "Net Velocity (" + units + "/s)";
            header.Cells[1, "G"] = "X Acceleration (" + units + "/s^2)";
            header.Cells[1, "H"] = "Y Acceleration (" + units + "/s^2)";
            header.Cells[1, "I"] = "Net Acceleration (" + units + "/s^2)";
            header.Cells[1, "J"] = "Volume (" + units + "^3)";
        }

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
    }
}
