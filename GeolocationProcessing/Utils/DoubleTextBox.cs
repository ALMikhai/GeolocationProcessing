using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace GeolocationProcessing.Utils
{
    class DoubleTextBox : TextBox
    {
        public double Value { get; private set; }

        public DoubleTextBox()
        {
            MaxLength = 15;
            PreviewTextInput += NumberValidationTextBox;
            TextChanged += OnTextBoxChange;
        }

        protected void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            double d;
            if (!double.TryParse((Text + e.Text), out d) || d < 0.0)
                e.Handled = true;
        }

        protected void OnTextBoxChange(object sender, TextChangedEventArgs e)
        {
            if (Text == "" || Text == ",")
                Text = "0";
            Value = Convert.ToDouble(Text);
        }
    }
}
