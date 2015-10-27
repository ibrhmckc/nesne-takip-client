using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using AviFile;

namespace Yazlab_Client.tools
{
    class image_convertions
    {
        public byte[] jpeg_to_byte_array(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return ms.ToArray();
        }

        public Image byte_array_to_image(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }

        // Aforge kütüphanesi ile bu şekilde yapılıyor.
        /*public void create_video_from_bitmaps(Bitmap[] BitmapStream,int Height,int Width)
        {
            // instantiate AVI writer, use WMV3 codec
            VideoFileWriter writer = new VideoFileWriter();
            // create new AVI file and open it
            writer.Open("C:\\yazlab\\processed_frames\\result.avi", Width, Height);
            // create frame image

            foreach(Bitmap Frame in BitmapStream)
            {
                writer.WriteVideoFrame(Frame);
            }
            writer.Close();

        }
        */
        public void create_video_from_bitmaps(Bitmap[] BitmapStream){
            try{
                AviManager aviManager = new AviManager(@"C:\\yazlab\\processed_frames\\result.avi", false);
                //add a new video stream and one frame to the new file
                VideoStream aviStream = aviManager.AddVideoStream(true, 0.2, BitmapStream[0]);

                foreach (Bitmap Frame in BitmapStream) {
                    aviStream.AddFrame(Frame);
                }
                aviManager.Close();
            }catch(Exception Exp){
            
                System.Windows.Forms.MessageBox.Show(Exp.ToString(),"AviFileHatası");
            }
        }


        public void create_video_from_bitmaps(Bitmap[] BitmapStream,double FrameRate)
        {
            try
            {
                AviManager aviManager = new AviManager(@"C:\\yazlab\\processed_frames\\result.avi", false);
                //add a new video stream and one frame to the new file
                VideoStream aviStream = aviManager.AddVideoStream(true, FrameRate, BitmapStream[0]);

                foreach (Bitmap Frame in BitmapStream)
                {
                    aviStream.AddFrame(Frame);
                }
                aviManager.Close();
            }
            catch (Exception Exp)
            {

                System.Windows.Forms.MessageBox.Show(Exp.ToString(), "AviFileHatası");
            }
        }
    }
}
