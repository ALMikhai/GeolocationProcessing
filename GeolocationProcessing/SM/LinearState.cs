using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Windows.Controls;

namespace GeolocationProcessing.SM
{
    class LinearState : State
    {
        private double _additionToMax = 0;
        private TextBox _textBox;
        private Label _label;

        public LinearState(MainWindow mainWindow, StateMachine stateMachine) : base(mainWindow, stateMachine)
        {
            _textBox = new TextBox
            {
                Margin = new System.Windows.Thickness(_standartMargine),
                Height = _standartHeight,
                TextAlignment = System.Windows.TextAlignment.Center,
                MaxLength = 15,
                Text = "0"
            };
            _textBox.PreviewTextInput += NumberValidationTextBox;
            _textBox.TextChanged += OnTextBoxChange;

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
            _mainWindow.ToolsPanel.Children.Add(_textBox);
        }

        public override int Process(double num, double maxNum)
        {
            return Convert.ToInt32(num / (maxNum * _additionToMax) * 255.0);
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            double d;
            if (!double.TryParse((_textBox.Text + e.Text), out d) || d < 0.0)
                e.Handled = true;
        }

        private void OnTextBoxChange(object sender, TextChangedEventArgs e)
        {
            _additionToMax = Convert.ToDouble(_textBox.Text);
        }
    }
}
