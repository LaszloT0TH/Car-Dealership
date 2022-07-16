using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace InputForms
{
    /// <summary>
    /// Arrangement of input fields, buttons, data grid
    /// </summary>
    internal class InputForm : Panel
    {
        Dictionary<string, InputField> fields;

        Button[] buttons;

        Action[] clickAction;

        public InputForm(Control parent)
        {
            Width = 1400;
            Height = 100;
            parent.Controls.Add(this);

            fields = new Dictionary<string, InputField>();

            buttons = new Button[4];
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i] = new Button();
                buttons[i].Text = "Button " + i;
                buttons[i].Font = new Font("sans-serif", 11f, FontStyle.Bold);
                buttons[i].Width = 150;
                buttons[i].Height = 35;

                this.Controls.Add(buttons[i]);

                buttons[i].Top = 25;
                buttons[i].Left = 25;
            }

            buttons[0].Click += OnClickZero;
            buttons[1].Click += OnClickFirst;
            buttons[2].Click += OnClickSecond;
            buttons[3].Click += OnClickThird;

            clickAction = new Action[4];
        }

        public string this[string name]
        {
            get { return GetValue(name); }
        }

        public string GetValue(string name)
        {
            if (fields.ContainsKey(name))
            {
                return fields[name].Value;
            }
            return null;
        }


        public InputForm Add(string name, InputField field)
        {
            int y = 25 + (fields.Count * 50);
            fields.Add(name, field);
            field.Add(this);

            field.MoveTo(25, y);

            y += 60;
            for (int i = 0; i < buttons.Length; i++)
            {
                y += 0;
                buttons[i].Top = y;
                y += 50;
                Height = y;
            }

            y += 500;
            Height = y;
            return this;
        }

        public InputForm SetButtonPosition(int left)
        {
            buttons[0].Left = buttons[1].Left = buttons[2].Left = buttons[3].Left = left;
            return this;
        }

        public InputForm MoveTo(int x, int y)
        {
            Left = x;
            Top = y;
            return this;
        }

        public InputForm SetButtonZero(string text, bool visible = true)
        {
            buttons[0].Text = text;
            if (!visible)
            {
                buttons[0].Hide();
            }
            return this;
        }

        public InputForm SetButtonFirst(string text = "", bool visible = false)
        {
            buttons[1].Text = text;
            if (!visible)
            {
                buttons[1].Hide();
            }
            return this;
        }

        public InputForm SetButtonSecond(string text = "", bool visible = false)
        {
            buttons[2].Text = text;
            if (!visible)
            {
                buttons[2].Hide();
            }
            return this;
        }

        public InputForm SetButtonThird(string text = "", bool visible = false)
        {
            buttons[3].Text = text;
            if (!visible)
            {
                buttons[3].Hide();
            }
            return this;
        }


        public InputForm OnSubmitZero(Action actionFirst)
        {
            clickAction[0] += actionFirst;
            return this;
        }

        public InputForm OnSubmitFirst(Action actionSecond)
        {
            clickAction[1] += actionSecond;
            return this;
        }

        public InputForm OnSubmitSecond(Action actionThird)
        {
            clickAction[2] += actionThird;
            return this;
        }

        public InputForm OnSubmitThird(Action actionFourth)
        {
            clickAction[3] += actionFourth;
            return this;
        }


        void OnClickZero(object sender, EventArgs e)
        {
            if (clickAction[0] != null)
            {
                string error = GetError();
                if (error != null)
                {
                    string msg = $"Falsch ausgefüllt: {error}";
                    MessageBox.Show(msg, "Fehler");
                }
                else clickAction[0]();
            }
        }

        void OnClickFirst(object sender, EventArgs e)
        {
            clickAction[1]();
        }

        void OnClickSecond(object sender, EventArgs e)
        {
            clickAction[2]();
        }

        void OnClickThird(object sender, EventArgs e)
        {
            clickAction[3]();
        }


        string GetError()
        {
            foreach (string name in fields.Keys)
            {
                if (!fields[name].IsValid()) return name;
            }
            return null;
        }
    }
}
