using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using GeolocationProcessing.Utils;

namespace GeolocationProcessing.SM
{
    class LogState : State
    {
        private DoubleTextBox _additionToMax;
        private Label _additionToMaxlabel;

        private DoubleTextBox _logBase;
        private Label _logBaseLabel;

        public LogState(MainWindow mainWindow, StateMachine stateMachine) : base(mainWindow, stateMachine)
        {
            _additionToMax = new DoubleTextBox
            {
                Margin = new System.Windows.Thickness(_standartMargine),
                Height = _standartHeight,
                TextAlignment = System.Windows.TextAlignment.Center,
            };

            _additionToMaxlabel = new Label()
            {
                Height = _standartHeight,
                Content = "Addition to max",
                FontSize = _standartFontSize,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
            };

            _logBase = new DoubleTextBox
            {
                Margin = new System.Windows.Thickness(_standartMargine),
                Height = _standartHeight,
                TextAlignment = System.Windows.TextAlignment.Center,
            };

            _logBaseLabel = new Label()
            {
                Height = _standartHeight,
                Content = "Logarithm base",
                FontSize = _standartFontSize,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
            };
        }

        public override void Enter()
        {
            _mainWindow.ToolsPanel.Children.Add(_additionToMaxlabel);
            _mainWindow.ToolsPanel.Children.Add(_additionToMax);

            _mainWindow.ToolsPanel.Children.Add(_logBaseLabel);
            _mainWindow.ToolsPanel.Children.Add(_logBase);
        }

        public override int Process(double num, double maxNum)
        {
            var x = (_logBase.Value - 1) / (maxNum * _additionToMax.Value) * num + 1;
            return Convert.ToInt32(Math.Log(x, _logBase.Value) * 255.0);
        }
    }
}
