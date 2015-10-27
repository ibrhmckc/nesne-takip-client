using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;


namespace Yazlab_Server.tools
{
    public class convertions
    {

        public byte[] tiff_to_byte_array(System.Drawing.Image imageIn)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Tiff);
                return ms.ToArray();
            }
            catch (Exception Exp) {

                
               
                return null;
            }
        }

        public Image byte_array_to_image(byte[] byteArrayIn)
        {
            try
            {
                MemoryStream ms = new MemoryStream(byteArrayIn);
                Image returnImage = Image.FromStream(ms);
                return returnImage;
            }
            catch (Exception Exp)
            {

               
                return null;
            }
        }

        public int bit_array_to_int(bool[] ArrayToConvert) {
            int Result = 0;
            for (int x = 0; x < ArrayToConvert.Length; x++) {
                if (ArrayToConvert[x]) {
                    Result = Result + (int)Math.Pow(2,x);
                }
            }
            Console.WriteLine(ArrayToConvert + " = > " + Result);
            return Result;
            
        }

      
        
    }
}