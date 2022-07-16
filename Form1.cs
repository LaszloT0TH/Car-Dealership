using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Car_Dealership
{
    public partial class Form1 : Form
    {
        Label label;

        public Form1()
        {
            InitializeComponent();

            Codec.CreateAppEncryptionAndExtension();

            Login login = new Login();

            login.ShowDialog();

            Menu menu = new Menu();

            menu.ShowDialog();

            label = new Label();

            label.Visible = true;

            label.Location = new Point(320, 180);

            label.Size = new Size(125, 45);

            label.Text = "Auf Wiedersehen";

            label.Font = new Font("sans-serif", 15f);

            label.AutoSize = true;

            this.Controls.Add(label);
        }
    }
}