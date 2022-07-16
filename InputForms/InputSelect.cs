using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InputForms
{
    internal class InputSelect : InputField
    {
        ComboBox combo;

        public InputSelect(string text, string[] options, Control parent = null) : base(text, parent)
        {
            (input as ComboBox).Items.AddRange(options);
            (input as ComboBox).SelectedIndex = 0;
        }

        protected override Control CreateField()
        {
            combo = new ComboBox();

            combo.Font = new Font("sans-serif", 12f);

            combo.DropDownStyle = ComboBoxStyle.DropDownList;

            return combo;
        }

        public InputSelect SetSize(int x)
        {
            combo.Width = x;
            return this;
        }

        public InputSelect SelectIndex(int Index)
        {
            (input as ComboBox).SelectedIndex = Index;
            return this;
        }
    }
}
