using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
namespace Lib.Utils
{
    public class ImageUtils
    {
        public static MemoryStream redimensionarProporcionalmente(Stream fileContent, int width)
        {
            return redimensionarProporcionalmente(new Bitmap(fileContent), width);
        }

        public static MemoryStream redimensionarProporcionalmente(Bitmap originalBmp, int width)
        {
            // Calculate the new image dimensions
            decimal origWidth = originalBmp.Width;
            decimal origHeight = originalBmp.Height;
            decimal sngRatio = origWidth / origHeight;
            int newWidth = width;
            int newHeight = (int)(newWidth / sngRatio);


            // Create a new bitmap which will hold the previous resized bitmap
            Bitmap newBMP = new Bitmap(originalBmp, newWidth, newHeight);
            // Create a graphic based on the new bitmap
            Graphics oGraphics = Graphics.FromImage(newBMP);

            // Set the properties for the new graphic file
            oGraphics.SmoothingMode = SmoothingMode.AntiAlias; oGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            // Draw the new graphic based on the resized bitmap
            oGraphics.DrawImage(originalBmp, 0, 0, newWidth, newHeight);

            MemoryStream ms = new MemoryStream();
            newBMP.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

            // Once finished with the bitmap objects, we deallocate them.
            originalBmp.Dispose();
            newBMP.Dispose();
            oGraphics.Dispose();

            return ms;
        }
    }
}
