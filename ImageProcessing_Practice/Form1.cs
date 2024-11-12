using System.Reflection;
using System.Windows.Forms;
using WebCamLib;
using ImageProcess2;
using System.ComponentModel;
using HNUDIP;
using Microsoft.VisualBasic.Devices;

namespace ImageProcessing_Practice
{
    public partial class Form1 : Form
    {
        Bitmap loaded, processed, imageA, imageB, resultImage;
        Device[] mgaDevice;
        public Form1()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            loaded = new Bitmap(openFileDialog1.FileName);
            pictureBox1.Image = loaded;
        }

        private void pixelCopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processed = new Bitmap(loaded.Width, loaded.Height);
            Color pixel;
            for (int x = 0; x < loaded.Width; x++)
                for (int y = 0; y < loaded.Height; y++)
                {
                    pixel = loaded.GetPixel(x, y);
                    processed.SetPixel(x, y, pixel);
                }
            pictureBox2.Image = processed;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog();
        }

        private void saveFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            processed.Save(saveFileDialog1.FileName);
        }

        private void grayscalingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processed = new Bitmap(loaded.Width, loaded.Height);
            Color pixel;
            int ave;
            for (int x = 0; x < loaded.Width; x++)
                for (int y = 0; y < loaded.Height; y++)
                {
                    pixel = loaded.GetPixel(x, y);
                    ave = (int)(pixel.R + pixel.G + pixel.B) / 3;
                    Color gray = Color.FromArgb(ave, ave, ave);
                    processed.SetPixel(x, y, gray);
                }
            pictureBox2.Image = processed;
        }

        private void inversionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processed = new Bitmap(loaded.Width, loaded.Height);
            Color pixel;
            for (int x = 0; x < loaded.Width; x++)
                for (int y = 0; y < loaded.Height; y++)
                {
                    pixel = loaded.GetPixel(x, y);
                    Color inv = Color.FromArgb(255 - pixel.R, 255 - pixel.G, 255 - pixel.B);
                    processed.SetPixel(x, y, inv);
                }
            pictureBox2.Image = processed;
        }

        private void mirrorHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processed = new Bitmap(loaded.Width, loaded.Height);
            Color pixel;
            for (int x = 0; x < loaded.Width; x++)
                for (int y = 0; y < loaded.Height; y++)
                {
                    pixel = loaded.GetPixel(x, y);
                    processed.SetPixel(loaded.Width - x - 1, y, pixel);
                }
            pictureBox2.Image = processed;
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

            processed = new Bitmap(loaded.Width, loaded.Height);
            Color pixel;
            for (int x = 0; x < loaded.Width; x++)
                for (int y = 0; y < loaded.Height; y++)
                {
                    pixel = loaded.GetPixel(x, y);
                    processed.SetPixel(x, loaded.Height - y - 1, pixel);
                }
            pictureBox2.Image = processed;
        }

        private void sepiaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processed = new Bitmap(loaded.Width, loaded.Height);
            Color pixel;
            int red, green, blue;
            for (int x = 0; x < loaded.Width; x++)
                for (int y = 0; y < loaded.Height; y++)
                {
                    pixel = loaded.GetPixel(x, y);
                    red = (byte)Math.Min((0.393 * pixel.R) + (0.769 * pixel.G) + (0.189 * pixel.B), 255.0);
                    green = (byte)Math.Min((0.349 * pixel.R) + (0.686 * pixel.G) + (0.168 * pixel.B), 255.0);
                    blue = (byte)Math.Min((0.272 * pixel.R) + (0.534 * pixel.G) + (0.131 * pixel.B), 255.0);

                    Color sep = Color.FromArgb(red, green, blue);
                    processed.SetPixel(x, y, sep);
                }
            pictureBox2.Image = processed;
        }

        private void histogramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BasicDIP.Hist(ref loaded, ref processed);
            pictureBox2.Image = processed;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            mgaDevice = DeviceManager.GetAllDevices();
        }

        private void onToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mgaDevice[0].ShowWindow(pictureBox1);
        }

        private void offToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mgaDevice[0].Stop();
        }

        private void grayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //get 1 frame
            IDataObject data;
            Image bmap;
            mgaDevice[0].Sendmessage();
            data = Clipboard.GetDataObject();
            bmap = (Image)(data.GetData("System.Drawing.Bitmap", true));
            Bitmap b = new Bitmap(bmap);
            BitmapFilter.GrayScale(b);
            pictureBox2.Image = b;

            //processed = new Bitmap(b.Width, b.Height);
            //Color pixel;
            //int ave;
            //for (int x = 0; x < b.Width; x++)
            //    for (int y = 0; y < b.Height; y++)
            //    {
            //        pixel = b.GetPixel(x, y);
            //        ave = (int)(pixel.R + pixel.G + pixel.B) / 3;
            //        Color gray = Color.FromArgb(ave, ave, ave);
            //        processed.SetPixel(x, y, gray);
            //    }
            //pictureBox2.Image = processed;


        }
        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog2.ShowDialog();
        }

        private void openFileDialog2_FileOk(object sender, CancelEventArgs e)
        {
            imageB = new Bitmap(openFileDialog2.FileName);
            pictureBox1.Image = imageB;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog3.ShowDialog();
        }

        private void openFileDialog3_FileOk(object sender, CancelEventArgs e)
        {
            imageA = new Bitmap(openFileDialog3.FileName);
            pictureBox2.Image = imageA;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Color mygreen = Color.FromArgb(0, 0, 255);
            int greygreen = (mygreen.R + mygreen.G + mygreen.B) / 3;
            int threshold = 5;

            resultImage = new Bitmap(imageA.Width, imageA.Height);

            for (int x = 0; x < imageB.Width; x++)
            {
                for (int y = 0; y < imageB.Height; y++)
                {
                    Color pixel = imageB.GetPixel(x, y);
                    Color backpixel = imageA.GetPixel(x, y);
                    int grey = (pixel.R + pixel.G + pixel.B) / 3;
                    int subtractvalue = Math.Abs(grey - greygreen);
                    if (subtractvalue < threshold)
                        resultImage.SetPixel(x, y, backpixel);
                    else
                        resultImage.SetPixel(x, y, pixel);
                }
            }
            pictureBox3.Image = resultImage;
        }

        private void subtractToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer2.Enabled = true;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            IDataObject data;
            Image bmap;
            mgaDevice[0].Sendmessage();
            data = Clipboard.GetDataObject();
            bmap = (Image)(data.GetData("System.Drawing.Bitmap", true));
            Bitmap b = new Bitmap(bmap);

            Color mygreen = Color.FromArgb(0, 0, 255);
            int greygreen = (mygreen.R + mygreen.G + mygreen.B) / 3;
            int threshold = 5;

            resultImage = new Bitmap(imageA.Width, imageA.Height);

            for (int x = 0; x < b.Width; x++)
            {
                for (int y = 0; y < b.Height; y++)
                {
                    Color pixel = b.GetPixel(x, y);
                    Color backpixel = imageA.GetPixel(x, y);
                    int grey = (pixel.R + pixel.G + pixel.B) / 3;
                    int subtractvalue = Math.Abs(grey - greygreen);
                    if (subtractvalue < threshold)
                        resultImage.SetPixel(x, y, backpixel);
                    else
                        resultImage.SetPixel(x, y, pixel);
                }
            }
            pictureBox3.Image = resultImage;

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            HNUDIP.ImageProcess.Brightness(ref loaded, ref processed, trackBar1.Value);
            pictureBox2.Image = processed;
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            HNUDIP.ImageProcess.Equalisation(ref loaded, ref processed, trackBar2.Value / 100);
            pictureBox2.Image = processed;
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            HNUDIP.ImageProcess.Rotate(ref loaded, ref processed, trackBar3.Value);
            pictureBox2.Image = processed;
        }

        private void scaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HNUDIP.ImageProcess.Scale(ref loaded, ref processed, 100, 100);
            pictureBox2.Image = processed;
        }

        private void binaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processed = new Bitmap(loaded.Width, loaded.Height);
            Color pixel;
            int ave;
            for (int x = 0; x < loaded.Width; x++)
                for (int y = 0; y < loaded.Height; y++)
                {
                    pixel = loaded.GetPixel(x, y);
                    ave = (int)(pixel.R + pixel.G + pixel.B) / 3;
                    if (ave < 180)
                        processed.SetPixel(x, y, Color.White);
                    else
                        processed.SetPixel(x, y, Color.Black);
                }
            pictureBox2.Image = processed;
        }

        private void smoothToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processed = (Bitmap)loaded.Clone();
            BitmapFilter.Smooth(processed, 4);
            pictureBox2.Image = processed;
        }

        private void gaussianBlurToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processed = (Bitmap)loaded.Clone();
            BitmapFilter.GaussianBlur(processed, 4);
            pictureBox2.Image = processed;

        }

        private void sharpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processed = (Bitmap)loaded.Clone();
            BitmapFilter.Sharpen(processed, 11);
            pictureBox2.Image = processed;
        }

        private void meanRemovalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processed = (Bitmap)loaded.Clone();
            BitmapFilter.MeanRemoval(processed, 9);
            pictureBox2.Image = processed;
        }

        private void embossHorizontalVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processed = (Bitmap)loaded.Clone();
            BitmapFilter.EmbossHorzVert(processed);
            pictureBox2.Image = processed;
        }

        private void embossAllDirectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processed = (Bitmap)loaded.Clone();
            BitmapFilter.EmbossAllDirection(processed);
            pictureBox2.Image = processed;
        }

        private void embossVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processed = (Bitmap)loaded.Clone();
            BitmapFilter.EmbossVertical(processed);
            pictureBox2.Image = processed;
        }

        private void embossHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processed = (Bitmap)loaded.Clone();
            BitmapFilter.EmbossHorizontal(processed);
            pictureBox2.Image = processed;
        }

        private void lossyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processed = (Bitmap)loaded.Clone();
            BitmapFilter.Lossy(processed);
            pictureBox2.Image = processed;
        }

        private void embossLaplascianToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processed = (Bitmap)loaded.Clone();
            BitmapFilter.EmbossLaplacian(processed);
            pictureBox2.Image = processed;
        }
    }
}
