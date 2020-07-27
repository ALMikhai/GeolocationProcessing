using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GeolocationProcessing
{
    class Geolocation
    {
        private GeolocationData _data;

        public Geolocation(string path)
        {
            _data = new GeolocationData(path);
        }

        public Bitmap CreateImage()
        {
            var result = new Bitmap(_data.GetWidth(), _data.GetHeight(), System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            for (int i = 0; i < _data.GetHeight(); i++)
            {
                var maxInLine = _data.GetMaxInLine(i);

                for (int j = 0; j < _data.GetWidth(); j++)
                {
                    int pixel_color = Convert.ToInt32(255 * Math.Log10(9 * _data.Data[i][j] / maxInLine + 1)) % 256;
                    result.SetPixel(j, i, Color.FromArgb(pixel_color, pixel_color, pixel_color));
                }
            }

            return result;
        }
    }
}
