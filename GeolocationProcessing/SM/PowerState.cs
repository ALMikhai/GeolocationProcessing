using GeolocationProcessing.Utils;
using System;
using System.Windows.Controls;

namespace GeolocationProcessing.SM
{
    class PowerState : State
    {
        private DoubleTextBox _Power;
        private Label _label;

        public PowerState(MainWindow mainWindow, StateMachine stateMachine) : base(mainWindow, stateMachine)
        {
            _Power = new DoubleTextBox
            {
                Margin = new System.Windows.Thickness(_standartMargine),
                Height = _standartHeight,
                TextAlignment = System.Windows.TextAlignment.Center,
                Text = "1"
            };

            _label = new Label()
            {
                Height = _standartHeight,
                Content = "Power",
                FontSize = _standartFontSize,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
            };
        }

        public override void Enter()
        {
            _mainWindow.ToolsPanel.Children.Add(_label);
            _mainWindow.ToolsPanel.Children.Add(_Power);
        }

        public override int Process(double num, double maxNum)
        {
            return Convert.ToInt32(Math.Pow((num / maxNum), _Power.Value) * 255.0);
        }

        public override string GetDescription()
        {
            return $"Power filter; Power = {_Power.Value}";
        }
    }
}
