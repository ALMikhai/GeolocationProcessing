using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

namespace GeolocationProcessing
{
    class GeolocationData
    {
        public string Path { get; private set; }
        public List<List<double>> Data { get; private set; }

        public GeolocationData(string path)
        {
            Data = new List<List<double>>();
            Path = path;
            
            StreamReader sr = new StreamReader(Path);
            while (true)
            {
                var line = sr.ReadLine();
                if (line == null)
                    break;

                List<double> dataLine = new List<double>();
                foreach (var word in line.Split())
                {
                    if (word != "")
                    {
                        dataLine.Add(Convert.ToDouble(word.Replace('.', ',')));
                    }
                }

                Data.Add(dataLine);
            }
            sr.Close();
        }

        public int GetHeight()
        {
            try
            {
                return Data.Count;
            }
            catch
            {
                throw new OutOfMemoryException();
            }
        }

        public int GetWidth()
        {
            try
            {
                return Data[0].Count;
            }
            catch
            {
                throw new OutOfMemoryException();
            }
        }

        public double GetMaxInLine(int lineIndex)
        {
            var line = Data[lineIndex];
            var max = line[0];

            foreach (var cell in line)
            {
                if (max < cell)
                    max = cell;
            }

            return max;
        }
    }
}
