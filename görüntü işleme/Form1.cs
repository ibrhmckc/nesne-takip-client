using AForge;
using AForge.Imaging.Filters;
using AForge.Video.FFMPEG;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WMPLib;

namespace görüntü_işleme
{
    public partial class Form1 : Form
    {
        ServiceReference1.FileUploaderSoapClient srv = new ServiceReference1.FileUploaderSoapClient();
         ServiceReference2.FileUploaderSoapClient srv2 = new ServiceReference2.FileUploaderSoapClient();
        int TotalFrame = 0;
        int Videofps = 0;
        OpenFileDialog ofVideoYukle = new OpenFileDialog();
        FolderBrowserDialog fbdFramePath = new FolderBrowserDialog();
        byte[] yenilerr;
        Image[] resimler;
        string yol;

        public Form1()
        {
            InitializeComponent();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            btnGonder.Enabled = false;
            button1.Enabled = false;
            button2.Enabled = false;
            button4.Enabled = false;
            button3.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = ofVideoYukle.ShowDialog();


            if (result != System.Windows.Forms.DialogResult.OK)
            {
                MessageBox.Show("Video Seçilmedi");
                return;
            }

            axWindowsMediaPlayer1.URL = ofVideoYukle.FileName;
            axWindowsMediaPlayer1.Ctlcontrols.play();
            button2.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FileStream dosyamiz;
            StreamReader okuma;
            string yol = "C:\\manual_Jogging1.txt";

            dosyamiz = new FileStream(yol, FileMode.Open, FileAccess.Read);
            okuma = new StreamReader(dosyamiz);

            int n, a, b, c, d;
            int x1, x2, x3, x4;
            double xgec;

            System.Drawing.Graphics graphicsObj;

            graphicsObj = this.CreateGraphics();

            Pen myPen = new Pen(System.Drawing.Color.Red, 1);




            VideoFileReader reader = new VideoFileReader();
            IWMPMedia saniye = axWindowsMediaPlayer1.newMedia(ofVideoYukle.FileName);

            if (fbdFramePath.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            {
                MessageBox.Show("Klasör seçilmedi");
            }

            string str_FramePath = fbdFramePath.SelectedPath + @"\";
            reader.Open(ofVideoYukle.FileName);
            this.TotalFrame = (int)saniye.duration * reader.FrameRate;
            this.Videofps = reader.FrameRate;
            for (int i = 0; i < 307; i++)
            {



                textBox1.Text = okuma.ReadLine();
                textBox2.Text = textBox1.Text.Replace("		", "");

                n = Convert.ToInt32(textBox2.Text.Substring(0, 1));
                a = Convert.ToInt32(textBox2.Text.Substring(1, 3));
                b = Convert.ToInt32(textBox2.Text.Substring(4, 3));
                c = Convert.ToInt32(textBox2.Text.Substring(7, 2));
                d = Convert.ToInt32(textBox2.Text.Substring(9, 2));

                if (i > 9)
                {
                    if (i == 48 || i == 49 || i == 50 || i == 51 || i == 52 || i == 53 || i == 54 || i == 55)
                    {
                        n = Convert.ToInt32(textBox2.Text.Substring(0, 2));
                        a = Convert.ToInt32(textBox2.Text.Substring(2, 2));
                        b = Convert.ToInt32(textBox2.Text.Substring(4, 3));
                        c = Convert.ToInt32(textBox2.Text.Substring(7, 2));
                        d = Convert.ToInt32(textBox2.Text.Substring(9, 2));
                    }

                    else
                    {
                        n = Convert.ToInt32(textBox2.Text.Substring(0, 2));
                        a = Convert.ToInt32(textBox2.Text.Substring(2, 3));
                        b = Convert.ToInt32(textBox2.Text.Substring(5, 3));
                        c = Convert.ToInt32(textBox2.Text.Substring(8, 2));
                        d = Convert.ToInt32(textBox2.Text.Substring(10, 2));
                    }

                }

                if (i > 99)
                {
                    n = Convert.ToInt32(textBox2.Text.Substring(0, 3));
                    a = Convert.ToInt32(textBox2.Text.Substring(3, 3));
                    b = Convert.ToInt32(textBox2.Text.Substring(6, 3));
                    c = Convert.ToInt32(textBox2.Text.Substring(9, 2));
                    d = Convert.ToInt32(textBox2.Text.Substring(11, 2));

                }

                x1 = a - c;
                x2 = b - ((3 * d) / 2);
                x3 = 2 * c;
                xgec = (Math.Sqrt(Math.Pow(c, 2) + Math.Pow(d, 2))) * 3;
                x4 = Convert.ToInt32(xgec);


                Rectangle myRectangle = new Rectangle(x1, x2, x3, x4);
                Bitmap videoFrame = reader.ReadVideoFrame();


                using (Graphics grafik = Graphics.FromImage(videoFrame as Image))
                {

                    grafik.DrawEllipse(myPen, myRectangle);

                }


                videoFrame.Save(str_FramePath + "Image" + i + ".jpg");
                videoFrame.Dispose();
            }

            reader.Close();
            reader.Dispose();
            MessageBox.Show("Video Frame lere ayrildi");
            button4.Enabled = true;
        }


        private void button3_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfVideoKaydet = new SaveFileDialog();
            if (sfVideoKaydet.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            {
                MessageBox.Show("Kayit Yeri Seçilmedi");
                return;
            }

            int width = 100;
            int height = 100;
            Image ImgOrnek = (Image.FromFile("C:\\Users\\Halil\\Desktop\\newframes\\image0.jpg") as Bitmap).Clone() as Image;
            width = ImgOrnek.Width;
            height = ImgOrnek.Height;
            ImgOrnek.Dispose();
            VideoFileWriter writer = new VideoFileWriter();
            writer.Open(sfVideoKaydet.FileName, width, height, this.Videofps, VideoCodec.MPEG4);
            yol = sfVideoKaydet.FileName;
            Bitmap image = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            DirectoryInfo dir = new DirectoryInfo(fbdFramePath.SelectedPath + "\\");
            int FrameSayisi = dir.GetFiles().Length;
            for (int i = 0; i < FrameSayisi - 3; i++)
            {
                image = (Bitmap)Image.FromFile("C:\\Users\\Halil\\Desktop\\newframes\\image" + i + ".jpg");
                writer.WriteVideoFrame(image);
                
            }

            
            
            writer.Close();
            writer.Dispose();
            MessageBox.Show("Video Olusturuldu");
            btnGonder.Enabled = true;
        }
       

        private void btnGonder_Click(object sender, EventArgs e)
        {
            UploadFile(yol);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 305; i++)
            {
                Bitmap image = (Bitmap)System.Drawing.Image.FromFile("C:\\Users\\Halil\\Desktop\\frames\\image" + i + ".jpg");
                Bitmap resim = new Bitmap(image);
                Bitmap resimson = new Bitmap(image);

                ColorFiltering filter = new ColorFiltering();
               
                filter.Red = new IntRange(40, 87);
      
                filter.Green = new IntRange(43, 85);
     
                filter.Blue = new IntRange(63, 95);

                filter.ApplyInPlace(resim);
                nesnebul(resim, i, resimson);
            }
            MessageBox.Show("Yeni Resimler Oluşturuldu");
            button3.Enabled = true;
        }

        public void nesnebul(Bitmap image, int index, Bitmap imageson)
        {
            AForge.Imaging.BlobCounter blobCounter = new AForge.Imaging.BlobCounter();
            blobCounter.MinWidth = 3;
            blobCounter.MinHeight = 3;
            blobCounter.FilterBlobs = true;
            blobCounter.ObjectsOrder = AForge.Imaging.ObjectsOrder.Size;
            blobCounter.ProcessImage(image);
            Rectangle[] rects = blobCounter.GetObjectsRectangles();

            foreach (Rectangle recs in rects)
            {
                if (rects.Length > 0)
                {
                    Rectangle objectRect = rects[0];
                    Graphics g = Graphics.FromImage(imageson);
                    using (Pen pen = new Pen(Color.FromArgb(55, 160, 81), 5))
                    {
                        g.DrawEllipse(pen, objectRect);
                    }
                    g.Dispose();
                }//end if
            }//end foreach
            imageson.Save("C:\\Users\\Halil\\Desktop\\newframes\\image" + index + ".jpg");
        }//end nesnebul

        private void UploadFile(string filename)
        {
            try
            {
                // get the exact file name from the path
                String strFile = System.IO.Path.GetFileName(filename);

                FileInfo fInfo = new FileInfo(filename);

              
                long numBytes = fInfo.Length;
                double dLen = Convert.ToDouble(fInfo.Length / 1000000);

             
                if (dLen < 15)
                {
                 
                    FileStream fStream = new FileStream(filename, FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fStream);

                    // convert the file to a byte array
                    byte[] data = br.ReadBytes((int)numBytes);
                    br.Close();

                    // pass the byte array (file) and file name to the web service
                    string sTmp = srv.UploadFile(data, strFile);
                    fStream.Close();
                    fStream.Dispose();

                    // this will always say OK unless an error occurs,
                    // if an error occurs, the service returns the error message
                    MessageBox.Show("Webservis Video Gönderme Cevabı: " + sTmp, "Dosya Yükleme");
                }
                else
                {
                    // Display message if the file was too large to upload
                    MessageBox.Show("Limit Aşımı.", "Dosya Boyutu");
                }
            }
            catch (Exception ex)
            {
                // display an error message to the user
                MessageBox.Show(ex.Message.ToString(), "Yükleme Hatası");
            }
        }
    
        //public string UploadFile1(byte[] f, string fileName)
        //{
            
        //        // the byte array argument contains the content of the file
        //        // the string argument contains the name and extension
        //        // of the file passed in the byte array

        //        // instance a memory stream and pass the
        //        // byte array to its constructor
        //        MemoryStream ms = new MemoryStream(f);

        //    // instance a filestream pointing to the 
        //    // storage folder, use the original file name
        //    // to name the resulting file
        //    FileStream fs = new FileStream
        //        ("C:\\Users\\Halil\\Desktop\\" +
        //        fileName, FileMode.Create);

        //    // write the memory stream containing the original
        //    // file as a byte array to the filestream
        //    ms.WriteTo(fs);

        //    // clean up
        //    ms.Close();
        //    fs.Close();
        //    fs.Dispose();

        //    // return OK if we made it this far
        //    return "OK";

        //}
        public void videoAl()
        {
            System.IO.FileStream fs1 = null;
           
            byte[] b1 = null;
            b1 = srv2.DownloadFile("C:\\Users\\Halil\\Desktop\\Uploader\\Uploader\\Uploader\\Uploader\\TransientStorage\\Jogging.AVI");
            fs1 = new FileStream("D:\\Jogging.AVI", FileMode.Create);
            fs1.Write(b1, 0, b1.Length);
            fs1.Close();
            fs1 = null;

        }

     

        private void button5_Click(object sender, EventArgs e)
        {
            videoAl();
            MessageBox.Show("Video webservisten başarı ile alınmıştır.");
            button1.Enabled = true;
        }
    }
}
