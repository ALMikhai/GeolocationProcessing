using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace GeolocationProcessing
{
    class ColorsChartModel
    {
        static public int[] Data { get; set; }
        static public string Description { get; set; }

        public PlotModel MyModel
        {
            get
            {
                return Create();
            }
        }

        private PlotModel Create()
        {
            var model = new PlotModel();

            var columnItems = new List<ColumnItem>();
            var categoryItems = new List<string>();

            double sum = Data.Sum();
            double errorPercent = 0.0;
            double minError = 100.0;
            double maxError = 0.0;
            double bestValue = 100.0 / 256.0;

            for (int i = 0; i < 256; i++)
            {
                categoryItems.Add(i.ToString());
                var value = Data[i] / sum * 100.0;
                var error = Math.Abs(bestValue - value) / bestValue;
                if (error < minError)
                    minError = error;
                if (error > maxError)
                    maxError = error;
                errorPercent += error;
                columnItems.Add(new ColumnItem { Value = value });
            }

            var columnSeries = new ColumnSeries
            {
                ItemsSource = columnItems,
                LabelPlacement = LabelPlacement.Inside,
                LabelFormatString = "{0:.00}%"
            };
            model.Series.Add(columnSeries);

            model.Axes.Add(new CategoryAxis
            {
                Position = AxisPosition.Left,
                Key = "Axis",
                ItemsSource = categoryItems
            });

            model.Title = Description + $"; Error percent = {errorPercent / 256.0}; Max error percent = {maxError}; Min error percent = {minError}";

            return model;
        }
    }
}
