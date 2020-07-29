using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Windows.Controls;
using GeolocationProcessing.Utils;

namespace GeolocationProcessing.SM
{
    class LinearState : State
    {
        private DoubleTextBox _additionToMax;
        private Label _label;

        public LinearState(MainWindow mainWindow, StateMachine stateMachine) : base(mainWindow, stateMachine)
        {
            _additionToMax = new DoubleTextBox
            {
                Margin = new System.Windows.Thickness(_standartMargine),
                Height = _standartHeight,
                TextAlignment = System.Windows.TextAlignment.Center,
                Text = "1"
            };

            _label = new Label()
            {
                Height = _standartHeight,
                Content = "Addition to max",
                FontSize = _standartFontSize,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
            };
        }

        public override void Enter()
        {
            _mainWindow.ToolsPanel.Children.Add(_label);
            _mainWindow.ToolsPanel.Children.Add(_additionToMax);
        }

        public override int Process(double num, double maxNum)
        {
            return Convert.ToInt32(num / (maxNum * _additionToMax.Value) * 255.0);
        }

        public override string GetDescription()
        {
            return $"Linear filter; Addition to max = {_additionToMax.Value}";
        }
    }
}
