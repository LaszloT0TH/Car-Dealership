using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InputForms
{
    internal class InputField
    {
        Label label;

        protected Control input;

        string rule;

        TextBox textBox;

        public InputField(string text, Control parent = null)
        {
            label = new Label();
            label.Text = text;
            label.Font = new Font("sans-serif", 10f);
            label.AutoSize = true;

            input = CreateField();

            if (parent != null) Add(parent);
        }

        public string Value
        {
            set { input.Text = Value; }
            get { return input.Text; }
        }

        public InputField Add(Control parent)
        {
            parent.Controls.Add(label);
            parent.Controls.Add(input);
            return this;
        }

        public InputField MoveTo(int x, int y)
        {
            label.Top = y;
            input.Top = y + 20;
            label.Left = input.Left = x;
            return this;
        }

        public InputField AddRule(string rule)
        {
            this.rule = rule;
            return this;
        }

        public bool IsValid()
        {
            if (rule == null) return true;

            return Regex.IsMatch(Value, "^" + rule + "$");
        }

        protected virtual Control CreateField()
        {
            textBox = new TextBox();
            textBox.Font = new Font("sans-serif", 12f);
            textBox.Width = 300;
            return textBox;
        }

        public static void ClearTextBox(Control hParent)
        {
            foreach (Control cControl in hParent.Controls)
            {
                if (cControl.HasChildren == true)
                {
                    ClearTextBox(cControl);
                }
                if (cControl is TextBoxBase)
                {
                    cControl.Text = string.Empty;
                }
            }
        }
    }
}
