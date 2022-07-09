using System;
using System.IO;
using System.Drawing;
using Imazen.WebP;

namespace WebpConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            //System.Diagnostics.Debugger.Launch();

            if (args.Length == 0)
                return;

            try
            {
                Imazen.WebP.Extern.LoadLibrary.LoadWebPOrFail();

                foreach(var path in args)
                {
                    Convert(path);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey();
            }
        }

        private static void Convert(string path)
        {
            byte[] fileBytes = File.ReadAllBytes(path);
            string outFile = FormattableString.Invariant($"{Path.GetFileNameWithoutExtension(path)}.jpg");
            File.Delete(outFile);

            FileStream outStream = new FileStream(outFile, FileMode.Create);
            SimpleDecoder decoder = new SimpleDecoder();

            Bitmap outBitmap = decoder.DecodeFromBytes(fileBytes, fileBytes.LongLength);
            outBitmap.Save(outStream, System.Drawing.Imaging.ImageFormat.Jpeg);
            outStream.Close();
        }
    }
}
