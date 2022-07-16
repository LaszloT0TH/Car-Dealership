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
    public partial class UsernamesAndPasswordsList : Form
    {
        InputForm form;

        List<Usernames_And_Passwords> usernames_And_Passwords;

        string[] UAPArray;

        private void Usernames_And_Passwords_List()
        {
            form = new InputForm(this);
            form.Add("UAP", new InputSelect("Benutzernamens und Passwörters:", UAPArray).SetSize(350))
                .MoveTo(10, 10)
                .SetButtonZero("Löschen")
                .SetButtonFirst("Neu", Visible = true)
                .SetButtonSecond("Menu", Visible = true)
                .SetButtonThird()
                .OnSubmitZero(() =>
                {
                    DeleteFromTheList();

                    MessageBox.Show(form["UAP"]);
                    InputField.ClearTextBox(this);
                })
                .OnSubmitFirst(() =>
                {
                    UsernamesAndPasswords usernamesAndPasswords = new UsernamesAndPasswords();
                    this.Visible = false;
                    usernamesAndPasswords.Visible = false;
                    usernamesAndPasswords.ShowDialog();
                })
                .OnSubmitSecond(() =>
                {
                    Menu menu = new Menu();
                    this.Visible = false;
                    menu.Visible = false;
                    menu.ShowDialog();
                });
        }

        private void DeleteFromTheList()
        {
            string[] record = form["UAP"].Split(';');
            string DeleteUsername = record[0];

            Usernames_And_Passwords.WriteDeleteUsernameListTextOrBin(usernames_And_Passwords, DeleteUsername,
                Codec.LevelOfAppEncryption, Codec.TextTrue_BinFalse);
        }

        private void CreateSelectList()
        {
            UAPArray = new string[usernames_And_Passwords.Count];
            for (int i = 0; i < usernames_And_Passwords.Count; i++)
            {
                UAPArray[i] += usernames_And_Passwords[i].usernames
                     + ";" + usernames_And_Passwords[i].passwords;
            }
        }

        public UsernamesAndPasswordsList()
        {
            InitializeComponent();

            usernames_And_Passwords = Usernames_And_Passwords.ReadListTextOrBin(Codec.LevelOfAppEncryption, Codec.TextTrue_BinFalse);
            
            CreateSelectList();

            Usernames_And_Passwords_List();
        }
    }
}
