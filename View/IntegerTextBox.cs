using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Controls;
using System.Windows.Input;

namespace Guitab.View
{
    // https://stackoverflow.com/questions/27973220/wpf-how-to-create-a-custom-textbox-with-validation-and-binding

    // https://stackoverflow.com/questions/841293/where-is-the-wpf-numeric-updown-control says that we could use
    // https://github.com/xceedsoftware/wpftoolkit but instead let's implement it ourselves just for practice
    class IntegerTextBox : TextBox
    {
        string oldText;

        public IntegerTextBox()
        {
            // too many events apart from input, e.g. paste, to trap and cancel previewed events reliably
            // so instead inspect after any/every change and undo if invalid
            this.TextChanged += IntegerTextBox_TextChanged;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.TextWrapping = System.Windows.TextWrapping.NoWrap;
            this.AcceptsReturn = false;
        }

        private void IntegerTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string newText = this.Text;
            if (!isValid(newText))
            {
                this.Text = oldText;
            }
            else
            {
                oldText = newText;
            }
        }

        bool isValid(string text)
        {
            if (string.IsNullOrEmpty(text))
                return false;
            int result;
            if (!int.TryParse(text, out result))
                return false;
            if (MinValue.HasValue && (MinValue.Value > result))
                return false;
            return true;
        }

        internal int IntegerValue
        {
            get { return int.Parse(this.Text); }
            set { oldText = this.Text = value.ToString("D"); }
        }

        public int? MinValue { get; set; }
    }
}
