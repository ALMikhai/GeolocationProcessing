using GeolocationProcessing.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GeolocationProcessing.SM
{
    class CustomState : State
    {
        private DoubleTextBox _Power;
        private Label _powerLabel;

        private DoubleTextBox _logBase;
        private Label _logBaseLabel;

        public CustomState(MainWindow mainWindow, StateMachine stateMachine) : base(mainWindow, stateMachine)
        {
            _Power = new DoubleTextBox
            {
                Margin = new System.Windows.Thickness(_standartMargine),
                Height = _standartHeight,
                TextAlignment = System.Windows.TextAlignment.Center,
                Text = "1"
            };

            _powerLabel = new Label()
            {
                Height = _standartHeight,
                Content = "Power",
                FontSize = _standartFontSize,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
            };

            _logBase = new DoubleTextBox
            {
                Margin = new System.Windows.Thickness(_standartMargine),
                Height = _standartHeight,
                TextAlignment = System.Windows.TextAlignment.Center,
                Text = "2"
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
            _mainWindow.ToolsPanel.Children.Add(_powerLabel);
            _mainWindow.ToolsPanel.Children.Add(_Power);

            _mainWindow.ToolsPanel.Children.Add(_logBaseLabel);
            _mainWindow.ToolsPanel.Children.Add(_logBase);
        }

        public override int Process(double num, double maxNum)
        {
            return Convert.ToInt32(Math.Pow(Math.Log((_logBase.Value - 1) * num / maxNum + 1, _logBase.Value), _Power.Value) * 255.0);
        }

        public override string GetDescription()
        {
            return $"Custom filter; Power = {_Power.Value}; Logarithm base = {_logBase.Value}";
        }
    }
}
