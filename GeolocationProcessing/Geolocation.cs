using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace GeolocationProcessing
{
    class Geolocation
    {
        private GeolocationData _data;
        private object _locker;

        public Geolocation(string path)
        {
            _data = new GeolocationData(path);
            _locker = new object();
        }

        public Bitmap CreateImage(int threadNumber)
        {
            var result = new Bitmap(_data.GetWidth(), _data.GetHeight(), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            List<Thread> threads = new List<Thread>(threadNumber);
            List<ImagePart> imageParts = new List<ImagePart>();

            for (int i = 0; i <= threadNumber - 1; i++)
            {
                int from = (_data.GetHeight() / threadNumber) * i;
                int to = (_data.GetHeight() / threadNumber) * (i + 1);

                ImagePart imagePart = new ImagePart(_data, from, to, result, _locker);
                imageParts.Add(imagePart);

                if (i == threadNumber - 1)
                {
                    imagePart.Process();
                }
                else
                {
                    Thread thread = new Thread(new ThreadStart(imagePart.Process));
                    thread.Start();
                    threads.Add(thread);
                }
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }

            return result;
        }
    }

    class ImagePart
    {
        private Bitmap _bitmap;
        private GeolocationData _data;
        private int _from = 0;
        private int _to = 0;
        private object _locker;

        public ImagePart(GeolocationData data, int from, int to, Bitmap bitmap, object locker)
        {
            _data = data;
            _from = from;
            _to = to;
            _bitmap = bitmap;
            _locker = locker;
        }

        public void Process()
        {
            for (int i = _from; i < _to; i++)
            {
                var maxInLine = _data.GetMaxInLine(i);
                for (int j = 0; j < _data.GetWidth(); j++)
                {
                    int pixelColor = Convert.ToInt32(255 * Math.Log10(9 * _data.Data[i][j] / maxInLine + 1)) % 256;
                    lock (_locker)
                    {
                        _bitmap.SetPixel(j, i, Color.FromArgb(pixelColor, pixelColor, pixelColor));
                    }
                }
            }
        }
    }
}
