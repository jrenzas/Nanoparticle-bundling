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
using System.Drawing.Imaging;
using System.Runtime.InteropServices;


namespace Bundling_Utility
{
    public partial class mainForm : Form
    {
        Bitmap bitMap;
        int imageHeight;
        int imageWidth;
        int currThreshold;
        List<string> dataList = new List<string>();
        List<string> rawDataList = new List<string>();
        bool imageOpened = false;
        double pixelSizeX;
        double pixelSizeY;
        double imageSizeUmX=0;
        double imageSizeUmY=0;
        string originalPath;
        bool direction = true;

        double percentCov = 0.0;

        public mainForm()
        {
            InitializeComponent();
        }

        private void Threshold()
        {
            bitMap = Bitonal();
  
            pictureBox1.Image = (Image)bitMap;
            lblCoverage.Text = "Coverage: " + Math.Round(percentCov,1).ToString() +"%" ;
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            openFileDialog.Title = "Select Image File for Processing";
            openFileDialog.DefaultExt = ".bmp";
            openFileDialog.Filter = "Image Files(*.bmp;*.jpg)|*.bmp;*.jpg";
            openFileDialog.Multiselect = false;
            DialogResult dlgResult = openFileDialog.ShowDialog();
            if (dlgResult == DialogResult.OK && openFileDialog.CheckFileExists && (openFileDialog.SafeFileName.EndsWith(".jpg") || openFileDialog.SafeFileName.EndsWith(".bmp")))
            {
                string fName = openFileDialog.FileName;
                bitMap = new Bitmap(fName);
                imageHeight = bitMap.Height;
                imageWidth = bitMap.Width;
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox1.Image = (Image)bitMap;
                btnProcess.Enabled = true;
                imageOpened = true;
                originalPath = fName;

                try
                {
                    string sName;
                    if (fName.EndsWith(".bmp"))
                    {
                        sName = fName.Replace(".bmp", "-bmp.hdr");
                        //else if(fName.EndsWith(".jpg"))...
                        string[] lines = System.IO.File.ReadAllLines(sName);
                        foreach (string line in lines)
                            if (line.Contains("PixelSizeX="))
                            {
                                string[] split = line.Split(new Char[] { '=', 'e' });
                                pixelSizeX = double.Parse(split[3]);
                                int exp = int.Parse(split[4]);
                                imageSizeUmX = imageWidth * pixelSizeX * Math.Pow(10,exp+6); //in um
                            }
                            else if (line.Contains("PixelSizeY="))
                            {
                                string[] split = line.Split(new Char[] { '=', 'e' });
                                pixelSizeY = double.Parse(split[3]);
                                int exp = int.Parse(split[4]);
                                imageSizeUmY = imageWidth * pixelSizeY * Math.Pow(10, exp + 6); //in um
                            }
                        lblImageSize.Text = "Image Size: " + Math.Round(imageSizeUmX, 2).ToString() + " um x " + Math.Round(imageSizeUmY, 2).ToString() + " um";
                    }
                }
                catch { ; }

            }
            else MessageBox.Show("Invalid file. Try again.");
        }



        public void BitonalThreshold(Rectangle cropArea)
        {
            bitMap.Dispose();
            bitMap = new Bitmap(originalPath);
            int threshold = currThreshold;
            int overThres = 0;
            BitmapData sourceData = bitMap.LockBits(cropArea,
                                        ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];
            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
            bitMap.UnlockBits(sourceData);
            for (int k = 0; k + 4 < pixelBuffer.Length; k += 4)
            {
                if (pixelBuffer[k] + pixelBuffer[k + 1] +
                     pixelBuffer[k + 2] <= threshold) { if (direction) overThres++; }
                else
                {
                    if(!direction) overThres++;
                }
            }
            percentCov = 100.0 * ((double)overThres) / ((double)cropArea.Width * (double)cropArea.Height);
        }

        public Bitmap Bitonal()
        {
            bitMap.Dispose();
            bitMap = new Bitmap(originalPath);
            Rectangle cropRect = new Rectangle(0, 0, bitMap.Width, bitMap.Height);
            Color darkColor = Color.Black;
            Color lightColor = Color.Yellow;
            int threshold = currThreshold;
            int overThres = 0;
            BitmapData sourceData = bitMap.LockBits(cropRect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];
            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
            bitMap.UnlockBits(sourceData);
            progressBar1.Maximum = pixelBuffer.Length;
            progressBar1.Visible = true;
            lblProgress.Visible = true;
            if (!direction)
            {
                for (int k = 0; k + 4 < pixelBuffer.Length; k += 4)
                {
                    if (k > 200 && (progressBar1.Value < k - 200)) progressBar1.Value = k;
                    if (pixelBuffer[k] + pixelBuffer[k + 1] +
                         pixelBuffer[k + 2] <= threshold)
                    {
                        pixelBuffer[k] = darkColor.B;
                        pixelBuffer[k + 1] = darkColor.G;
                        pixelBuffer[k + 2] = darkColor.R;
                    }
                    else
                    {
                        overThres++;
                        pixelBuffer[k] = lightColor.B;
                        pixelBuffer[k + 1] = lightColor.G;
                        pixelBuffer[k + 2] = lightColor.R;
                    }
                }
            }
            else
            {
                for (int k = 0; k + 4 < pixelBuffer.Length; k += 4)
                {
                    if (k > 200 && (progressBar1.Value < k - 200)) progressBar1.Value = k;
                    if (pixelBuffer[k] + pixelBuffer[k + 1] +
                         pixelBuffer[k + 2] <= threshold)
                    {
                        overThres++;
                        pixelBuffer[k] = lightColor.B;
                        pixelBuffer[k + 1] = lightColor.G;
                        pixelBuffer[k + 2] = lightColor.R;
                    }
                    else
                    {
                        
                        pixelBuffer[k] = darkColor.B;
                        pixelBuffer[k + 1] = darkColor.G;
                        pixelBuffer[k + 2] = darkColor.R;
                    }
                }
            }
            Bitmap resultBitmap = new Bitmap(bitMap.Width, bitMap.Height);
            BitmapData resultData = resultBitmap.LockBits(new Rectangle(0, 0,
                                    resultBitmap.Width, resultBitmap.Height),
                                    ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            Marshal.Copy(pixelBuffer, 0, resultData.Scan0, pixelBuffer.Length);
            resultBitmap.UnlockBits(resultData);
            percentCov = 100.0*((double)overThres) / ((double)imageWidth * (double)imageHeight);
            progressBar1.Visible = false;
            lblProgress.Visible = false;
            return resultBitmap;
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            //int levels = (int)numericUpDownLevels.Value;
            string str = "";
            btnOpenFile.Enabled = false;
            btnOriginal.Enabled = false;
            btnProcess.Enabled = false;
            btnThreshold.Enabled = false;
            currThreshold = (int)numericUpDownThreshold.Value;

            lblProgress.Visible = true;
            progressBar1.Visible = true;
            progressBar1.Value = 0;
            
            //for (int n = levels; n > 0; n--) progressBar1.Maximum += n * n;
            //for (int i = 1; i < (levels + 1); i++)
            Random rand = new Random();
            List<int> randList = new List<int>();
            int levelMax = 513;
            int sampleCount = 100;
            for (int v = 2; v < levelMax; v *= 2) progressBar1.Maximum += sampleCount;

            for (int i = 2; i < levelMax; i *= 2)
            {
                List<double> rawList = new List<double>();
                int width = imageWidth/i;
                int height = imageHeight/i;
                

                if (i*i > sampleCount)
                {
                    for (int z = 0; z < sampleCount; z++)
                    {
                        Rectangle cropArea = new Rectangle(rand.Next(0,i)*width, rand.Next(0,i)*height, width, height);
                        BitonalThreshold(cropArea);
                        rawList.Add(percentCov);
                        str += Math.Round(percentCov, 2).ToString() + ",";
                        progressBar1.Value++;
                    }
                }
                else
                {
                    for (int x = 0; (x + width) < (imageWidth + 1); x += width)
                    {
                        for (int y = 0; (y + height) < (imageHeight + 1); y += height)
                        {
                            Rectangle cropArea = new Rectangle(x, y, width, height);
                            BitonalThreshold(cropArea);
                            rawList.Add(percentCov);
                            str += Math.Round(percentCov, 2).ToString() + ",";
                            progressBar1.Value++;
                        }
                    }
                }

                double mean = 0;
                double stdev = 0;

                foreach (double d in rawList)
                {
                    mean += d;
                }
                mean = mean / rawList.Count;

                //calc standard dev.
                foreach (double d in rawList)
                {
                    stdev += Math.Pow(d - mean,2);
                }
                stdev = Math.Sqrt(stdev / rawList.Count);
                stdev = 100 * stdev / mean;
                string str2 = i.ToString() + "," + Math.Round(mean, 2).ToString() + "," + Math.Round(stdev, 2).ToString();
                if (imageSizeUmX > 0.01 && imageSizeUmY > 0.01) str2 += "," + Math.Round((imageSizeUmX / (double)i), 2).ToString() + "," + Math.Round((imageSizeUmY / (double)i), 2).ToString();
                dataList.Add(str2);
                rawDataList.Add(str);
                str = "";

            }
            lblProgress.Visible = false;
            progressBar1.Visible = false;
            SaveData();

            btnOpenFile.Enabled = true;
            btnOriginal.Enabled = true;
            btnProcess.Enabled = true;
            btnThreshold.Enabled = true;
        }

        private void SaveData()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.OverwritePrompt = true;
            saveFileDialog.FileName = "Roping Data";
            saveFileDialog.DefaultExt = "csv";
            saveFileDialog.Filter = "Comma-delimited text files (*.csv)|*.csv|All files (*.*)|*.*";
            saveFileDialog.InitialDirectory = Bundling_Utility.Properties.Settings.Default.defaultDir;

            try
            {
                if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    System.IO.StreamWriter file = new System.IO.StreamWriter(saveFileDialog.OpenFile());
                    WriteDataToFile(file);
                    Bundling_Utility.Properties.Settings.Default.defaultDir = System.IO.Path.GetDirectoryName(saveFileDialog.FileName);
                    Bundling_Utility.Properties.Settings.Default.Save();
                }
                else MessageBox.Show("File not saved.");
            }
            catch
            {
                MessageBox.Show("File open in another program. Close program or use a different file name and try again.");
                return;
            }
        }

        private void WriteDataToFile(System.IO.StreamWriter file)
        {
            try
            {
                string str = "Sample:," + textBoxName.Text + ",,Level,Mean,% Standard Deviation";
                if(imageSizeUmX > 0.01 && imageSizeUmY > 0.01) str += ",Size X (um),Size Y (um)";
                file.WriteLine(str);
                foreach (string s in dataList)
                {
                    file.WriteLine(",,," + s);
                }
                file.WriteLine();
                file.WriteLine("Raw Data Below:");
                foreach (string s in rawDataList) file.WriteLine(s);

                file.Close();
                dataList.Clear();
                rawDataList.Clear();
            }
            catch { MessageBox.Show("Error. Check file name and data integrity."); return; }
        }

        private void btnOriginal_Click(object sender, EventArgs e)
        {
            if (imageOpened) pictureBox1.Image = (Image)bitMap;
        }

        private void btnThreshold_Click(object sender, EventArgs e)
        {
            currThreshold = (int)numericUpDownThreshold.Value;
            if (imageOpened) Threshold();
        }

/*        private void numericUpDownLevels_ValueChanged(object sender, EventArgs e)
        {
            if (imageSizeUmX > 0.001)
            {
                double smallestXum = imageSizeUmX / (double)numericUpDownLevels.Value;
                double smallestYum = imageSizeUmY / (double)numericUpDownLevels.Value;
                lblSmallestSize.Text = "Smallest Subset Size: " + Math.Round(smallestXum, 1).ToString() + " um x " + Math.Round(smallestYum, 1).ToString() + " um";
            }

        }*/

    }
}
