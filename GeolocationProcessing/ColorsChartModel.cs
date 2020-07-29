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
            var model = new PlotModel { Title = Description };

            var columnItems = new List<ColumnItem>();
            var categoryItems = new List<string>();

            double sum = Data.Sum();

            for (int i = 0; i < 256; i++)
            {
                categoryItems.Add(i.ToString());
                columnItems.Add(new ColumnItem { Value = (Data[i] / sum * 100.0) });
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

            return model;
        }
    }
}
