using System;
using System.IO;
using System.Drawing;
using Imazen.WebP;
using System.Runtime.Versioning;
using OutputType = System.Drawing.Imaging.ImageFormat;
using System.Linq;

namespace WebpConverter
{
    [SupportedOSPlatform("Windows")]
    class Program
    {
        private enum OutputTypeOption
        {
            Png,
            Jpg
        }

        static void Main(string[] args)
        {
            System.Diagnostics.Debugger.Launch();

            if (args.Length == 0)
            {
                return;
            }

            OutputTypeOption outputTypeMode = OutputTypeOption.Png;
            string option = args.FirstOrDefault(o => o.StartsWith("-"));
            if (!string.IsNullOrWhiteSpace(option))
            {
                if (option == "-jpg")
                {
                    outputTypeMode = OutputTypeOption.Jpg;
                }
            }

            try
            {
                Imazen.WebP.Extern.LoadLibrary.LoadWebPOrFail();

                foreach (string path in args)
                {
                    if (path.StartsWith("-"))
                    {
                        continue;
                    }

                    Convert(path, outputTypeMode);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey();
            }
        }

        private static void Convert(string path, OutputTypeOption outputTypeOption)
        {
            byte[] fileBytes = File.ReadAllBytes(path);
            string outFile = FormattableString.Invariant($"{Path.GetDirectoryName(path)}{Path.DirectorySeparatorChar}{Path.GetFileNameWithoutExtension(path)}.{outputTypeOption.ToString().ToLowerInvariant()}");
            File.Delete(outFile);

            FileStream outStream = new FileStream(outFile, FileMode.Create);
            SimpleDecoder decoder = new SimpleDecoder();

            Bitmap outBitmap = decoder.DecodeFromBytes(fileBytes, fileBytes.LongLength);

            OutputType outputType = OutputType.Png;
            if (outputTypeOption == OutputTypeOption.Jpg)
            {
                outputType = OutputType.Jpeg;
            }

            outBitmap.Save(outStream, outputType);
            outStream.Close();
        }
    }
}
