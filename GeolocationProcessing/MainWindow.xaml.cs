using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using GeolocationProcessing.SM;

namespace GeolocationProcessing
{
    public partial class MainWindow : Window
    {
        private StateMachine _stateMachine;
        private LinearState _linearState;

        public MainWindow()
        {
            InitializeComponent();
            _stateMachine = new StateMachine();
            _linearState = new LinearState(this, _stateMachine);

            _stateMachine.Initialize(_linearState);
        }

        private void BroseFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFileName = openFileDialog.FileName;
                var geo = new Geolocation(selectedFileName);
                var bitmap = geo.CreateImage(8, _stateMachine.CurrentState); // TODO ввод количества потоков.

                using (MemoryStream memory = new MemoryStream())
                {
                    bitmap.Save(memory, ImageFormat.Png);
                    memory.Position = 0;
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = memory;
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.EndInit();
                    ResultImage.Source = bitmapImage;
                }
            }
        }

        private void SaveImage_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Image Files(*.JPG)|*.JPG";

            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName;
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create((BitmapSource)ResultImage.Source));
                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                encoder.Save(stream);
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Benchmark_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFileName = openFileDialog.FileName;
                var geo = new Geolocation(selectedFileName);
                StringBuilder result = new StringBuilder();
                
                for (int i = 1; i <= 16; i++)
                {
                    Stopwatch watch = new Stopwatch();
                    watch.Start();
                    geo.CreateImage(i, _stateMachine.CurrentState);
                    watch.Stop();
                    result.AppendLine
                        (
                            $"Кол-во потоков - {i}; " +
                            $"Время выполнения в миллисекундах - {watch.ElapsedMilliseconds}; " +
                            $"Время выполнения в секундах  - {watch.Elapsed.Seconds}; " +
                            $"Время выполнения в тиках  - {watch.ElapsedTicks}; "
                        );
                }

                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "TxT Files(*.TXT)|*.TXT";

                if (saveFileDialog.ShowDialog() == true)
                {
                    string filePath = saveFileDialog.FileName;
                    TextWriter textWriter = new StreamWriter(filePath);
                    textWriter.Write(result.ToString());
                    textWriter.Close();
                }
            }
        }
    }
}
