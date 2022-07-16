using InputForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Car_Dealership
{
    public partial class UsernamesAndPasswords : Form
    {
        InputForm form;

        Label label;

        Usernames_And_Passwords username_And_Passwords;

        const string Help = "Das Passwort  " +
                          "\nDie letzten beiden Zeichen des Passworts müssen die Nummer der Verkäufer-Identifikationsnummer sein" +
                          "\nBevor Sie ein neues Passwort erstellen, sehen Sie sich bitte die ID-Nummer des Verkäufers an" +
                          //"\n - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - " +
                          "\n";

        private void Usernames_And_PassWords()
        {
            this.Controls.Add(label);
            form = new InputForm(this);
            form.Add("Username", (new InputField("Neu Benutzername (Nummer und kleine Buchstaben)")).AddRule("[a-zäöüß0-9]{1,30}"))
                .Add("Pasword", (new InputField("Neu Passwort die letzte zwei Karakter muss salesId Nummer sein ")).AddRule("[A-ZÖÜÄa-zäöüß,.&@#0-9]{1,30}"))
                .MoveTo(10, 10)
                .SetButtonZero("Speichern")
                .SetButtonFirst("List", Visible = true)
                .SetButtonSecond("Menu", Visible = true)
                .SetButtonThird("Info", Visible = true)
                .OnSubmitZero(() =>
                {
                    WriteNewEntryEoTheUsernamesAndPasswordsList();

                    MessageBox.Show($"Geschpeichert\n\n{form["Username"]} {form["Pasword"]}");

                    InputField.ClearTextBox(this);
                })
                .OnSubmitFirst(() =>
                {
                    UsernamesAndPasswordsList usernamesAndPasswordsList = new UsernamesAndPasswordsList();
                    this.Visible = false;
                    usernamesAndPasswordsList.Visible = false;
                    usernamesAndPasswordsList.ShowDialog();
                })
                .OnSubmitSecond(() =>
                {
                    Menu menu = new Menu();
                    this.Visible = false;
                    menu.Visible = false;
                    menu.ShowDialog();
                })
                .OnSubmitThird(() =>
                {
                    label.Text = Help;
                });
        }

        private void WriteNewEntryEoTheUsernamesAndPasswordsList()
        {
            username_And_Passwords = new Usernames_And_Passwords()
            { usernames = form["Username"], passwords = form["Pasword"] };

            Usernames_And_Passwords.WriteAddTextOrBin(username_And_Passwords,
                Codec.LevelOfAppEncryption, Codec.TextTrue_BinFalse);
        }

        private void CreateLabel()
        {
            label = new Label();

            label.Visible = true;

            label.Location = new Point(35, 340);

            label.Size = new Size(125, 45);

            label.Font = new Font("sans-serif", 11f);

            label.AutoSize = true;
        }

        public UsernamesAndPasswords()
        {
            InitializeComponent();

            CreateLabel();

            Usernames_And_PassWords();
        }
    }
}
