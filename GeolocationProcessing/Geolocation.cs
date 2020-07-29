using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using GeolocationProcessing.SM;

namespace GeolocationProcessing
{
    class Geolocation
    {
        private GeolocationData _data;
        private Bitmap _bitmap;

        public Geolocation(string path)
        {
            _data = new GeolocationData(path);
        }

        public Bitmap CreateImage(int threadNumber, State currentState)
        {
            _bitmap = new Bitmap(_data.GetWidth(), _data.GetHeight(), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            var bitmapData = _bitmap.LockBits(new Rectangle(0, 0, _data.GetWidth(), _data.GetHeight()), System.Drawing.Imaging.ImageLockMode.ReadWrite, _bitmap.PixelFormat);
            var depth = Bitmap.GetPixelFormatSize(bitmapData.PixelFormat) / 8;
            
            var buffer = new byte[_data.GetWidth() * _data.GetHeight() * depth];

            List<Thread> threads = new List<Thread>(threadNumber);

            for (int i = 0; i <= threadNumber - 1; i++)
            {
                int from = (_data.GetHeight() / threadNumber) * i;
                int to = (_data.GetHeight() / threadNumber) * (i + 1);

                ImagePart imagePart = new ImagePart(_data, from, to, buffer, depth, currentState);

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

            Marshal.Copy(buffer, 0, bitmapData.Scan0, buffer.Length);

            _bitmap.UnlockBits(bitmapData);

            return _bitmap;
        }

        public int[] GetChartData()
        {
            var result = new int[256];

            for (int i = 0; i < _bitmap.Width; i++)
            {
                for (int j = 0; j < _bitmap.Height; j++)
                {
                    result[_bitmap.GetPixel(i, j).R]++;
                }
            }

            return result;
        }
    }

    class ImagePart
    {
        private GeolocationData _data;
        private int _from = 0;
        private int _to = 0;
        private byte[] _buffer;
        private int _depth;
        private State _currentState;

        public ImagePart(GeolocationData data, int from, int to, byte[] buffer, int depth, State currentState)
        {
            _data = data;
            _from = from;
            _to = to;
            _depth = depth;
            _buffer = buffer;
            _currentState = currentState;
        }

        public void Process()
        {
            for (int i = _from; i < _to; i++)
            {
                var maxInLine = _data.GetMaxInLine(i);
                for (int j = 0; j < _data.GetWidth(); j++)
                {
                    var offset = ((i * _data.GetWidth()) + j) * _depth;
                    int pixelColor = _currentState.Process(_data.Data[i][j], maxInLine);
                    _buffer[offset + 0] = _buffer[offset + 1] = _buffer[offset + 2] = (byte)pixelColor;
                }
            }
        }
    }
}
