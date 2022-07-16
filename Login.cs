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
    public partial class Login : Form
    {
        public static bool flag = false;

        InputForm form;

        List<Usernames_And_Passwords> usernames_And_Passwords;

        private void Start()
        {
            form = new InputForm(this);
            form.Add("Username", (new InputField("Geben Sie bitte Ihre Benutzername ein: chef")))
                .Add("Pasword", (new InputField("Geben Sie bitte Ihre Passwort ein: boss00")))
                .MoveTo(200, 150)
                .SetButtonZero("Ok")
                .SetButtonFirst()
                .SetButtonSecond()
                .SetButtonThird()
                .OnSubmitZero(() =>
                {
                    if (ChecksIfItIsOnTheList())
                    {
                        this.Close();
                    }
                });

        }

        private bool ChecksIfItIsOnTheList()
        {
            if (usernames_And_Passwords.Contains(new Usernames_And_Passwords { usernames = form["Username"], passwords = form["Pasword"] }))
            {
                flag = true;

                DateTime date = DateTime.Now;

                Logins log = new Logins()
                {
                    Username = form["Username"],
                    Password = form["Pasword"],
                    Date = date
                };

                Logins.WriteAddTextOrBin(log, Codec.LevelOfAppEncryption, Codec.TextTrue_BinFalse);

                return flag;
            }
            else
            {
                flag = false;
                MessageBox.Show("Geben Sie korrekte Details ein");
                InputField.ClearTextBox(this);
                return flag;
            }
        }

        public Login()
        {
            InitializeComponent();

            usernames_And_Passwords = Usernames_And_Passwords.ReadListTextOrBin(Codec.LevelOfAppEncryption, Codec.TextTrue_BinFalse);

            Start();
        }
    }
}
