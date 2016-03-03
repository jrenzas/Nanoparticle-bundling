//Bundling Utility written by Russ Renzas in 2014.
//Purpose: Determine aggregation/bundling level of nanoparticles or nanowires.
//Designed to work with scanning electron microscopy images from a Tescan Mira FE-SEM.
//Not validated or tested with any other type of input files.
//No attribution required. 
//Only interesting thing is the concept - in perfect dispersion, areal coverage should be relatively
//constant regardless of how zoomed in/out the image is. Large variation at higher zoom levels implies
//bundling or particle aggregation in some areas. 

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



        public double BitonalThreshold(Rectangle cropArea)
        {
            //bitMap.Dispose();
            //bitMap = new Bitmap(originalPath);
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
            return (100.0 * ((double)overThres) / ((double)cropArea.Width * (double)cropArea.Height));
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
            //string str = "";
            btnOpenFile.Enabled = false;
            btnOriginal.Enabled = false;
            btnProcess.Enabled = false;
            btnThreshold.Enabled = false;
            currThreshold = (int)numericUpDownThreshold.Value;

            lblProgress.Visible = true;
            progressBar1.Visible = true;
            progressBar1.Value = 0;
            
            int levelMax = 128; //KEY - CONTROLS # of LEVELS!!!

            int heightStepPx = imageHeight / levelMax;
            int widthStepPx = imageWidth / levelMax;

            double[,] covArray = new double[levelMax, levelMax];
            double meanMax = 0;
            double stdevMax = 0;

            progressBar1.Maximum = levelMax * levelMax;
            for (int i = 0; i < levelMax; i++)
                for (int j = 0; j < levelMax; j++)
                {
                    Rectangle cropArea = new Rectangle(widthStepPx*i, heightStepPx*j, widthStepPx, heightStepPx);
                    covArray[i, j] = BitonalThreshold(cropArea);
                    meanMax += covArray[i, j];
                    progressBar1.Value++;
                }
            meanMax = meanMax / (levelMax * levelMax);

            foreach (double d in covArray)
            {
                stdevMax += Math.Pow(d - meanMax, 2);
            }
            stdevMax = Math.Sqrt(stdevMax / covArray.Length);
            stdevMax = 100 * stdevMax / meanMax;
            //finished initialization
            string str = (levelMax).ToString() + "," + Math.Round(meanMax, 2).ToString() + "," + Math.Round(stdevMax, 2).ToString();
            if (imageSizeUmX > 0.01 && imageSizeUmY > 0.01) str += "," + Math.Round((imageSizeUmX / (double)((levelMax))), 2).ToString() + "," + Math.Round((imageSizeUmY / (double)(levelMax)), 2).ToString();
            dataList.Add(str);
            
            for (int currentLevel = levelMax/2; currentLevel > 1; currentLevel = currentLevel/2)
            {
                double[,] covArray2 = new double[currentLevel, currentLevel];
                for (int i = 0; i < currentLevel; i++)
                    for (int j = 0; j < currentLevel; j++)
                    {
                        covArray2[i, j] = (covArray[2 * i, 2 * j] + covArray[2 * i + 1, 2 * j] + covArray[2 * i, 2 * j + 1] + covArray[2 * i + 1, 2 * j + 1]) / 4;
                    }
                double stdev = 0;
                foreach (double d in covArray2)
                {
                    stdev += Math.Pow(d - meanMax, 2);
                }
                stdev = Math.Sqrt(stdev / covArray2.Length);
                stdev = 100 * stdev / meanMax;
                string str2 = currentLevel.ToString() + "," + Math.Round(meanMax, 2).ToString() + "," + Math.Round(stdev, 2).ToString();
                if (imageSizeUmX > 0.01 && imageSizeUmY > 0.01) str2 += "," + Math.Round((imageSizeUmX / (double)(currentLevel)), 2).ToString() + "," + Math.Round((imageSizeUmY / (double)(currentLevel)), 2).ToString();
                dataList.Insert(0, str2);
                covArray = covArray2;

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
                string str = "Sample:,% StdDev,,Level,Mean," +textBoxName.Text;
                if(imageSizeUmX > 0.01 && imageSizeUmY > 0.01) str += ",Size X (um),Size Y (um)";
                file.WriteLine(str);
                foreach (string s in dataList)
                {
                    file.WriteLine(",,," + s);
                }

                file.Close();
                dataList.Clear();
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

    }
}
