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
    public partial class Menu : Form
    {
        InputForm form;

        Label label;

        List<Logins> logins;

        const string DropDownMenuAddNewCustomer = "Neu Kunde hinzufügen";

        const string DropDownMenuAddNewCar = "Neu Auto hinzufügen";

        const string DropDownMenuAddOrDeleteNewSalasPerson = "Neu VerkäuferIn hinzufügen oder kündigen";

        const string DropDownMenuUsernamesAndPasswords = "Benutzernamen und Passwörter";

        const string DropDownMenuQueryingData = "Abfragen von Daten";

        const string DropDownMenuSettings = "Einstellungen";

        private void Start()
        {
            form = new InputForm(this);
            form.Add("Menü", new InputSelect("Menü:", DropDownMenu()).SetSize(200))
                .MoveTo(10, 10)
                .SetButtonZero("Ok")
                .SetButtonFirst()
                .SetButtonSecond()
                .SetButtonThird()
                .OnSubmitZero(() =>
                {
                    SelectedMenu();
                });
        }

        private string[] DropDownMenu()
        {
            Logins log = logins[logins.Count - 1];

            string[] SelMenu;

            if (log.Username == "chef")
            {
                SelMenu = new string[]
                {
                    DropDownMenuAddNewCustomer,
                    DropDownMenuAddNewCar,
                    DropDownMenuAddOrDeleteNewSalasPerson,
                    DropDownMenuUsernamesAndPasswords,
                    DropDownMenuQueryingData,
                    DropDownMenuSettings
                };
                return SelMenu;
            }
            else
            {
                SelMenu = new string[]
                {
                    DropDownMenuAddNewCustomer,
                    DropDownMenuAddNewCar
                };
                return SelMenu;
            }
        }

        private void SelectedMenu()
        {
            switch (form["Menü"])
            {
                case DropDownMenuAddNewCustomer:
                    {
                        NewCostumer newCostumer = new NewCostumer();
                        this.Visible = false;
                        newCostumer.Visible = false;
                        newCostumer.ShowDialog();
                        break;
                    }
                case DropDownMenuAddNewCar:
                    {
                        NewCar newCar = new NewCar();
                        this.Visible = false;
                        newCar.Visible = false;
                        newCar.ShowDialog();
                        break;
                    }
                case DropDownMenuAddOrDeleteNewSalasPerson:
                    {
                        NewSalesperson newSalesperson = new NewSalesperson();
                        this.Visible = false;
                        newSalesperson.Visible = false;
                        newSalesperson.ShowDialog();
                        break;
                    }
                case DropDownMenuUsernamesAndPasswords:
                    {
                        UsernamesAndPasswords usernamesAndPasswords = new UsernamesAndPasswords();
                        this.Visible = false;
                        usernamesAndPasswords.Visible = false;
                        usernamesAndPasswords.ShowDialog();
                        break;
                    }
                case DropDownMenuQueryingData:
                    {
                        Query query = new Query();
                        this.Visible = false;
                        query.Visible = false;
                        query.ShowDialog();
                        break;
                    }
                case DropDownMenuSettings:
                    {
                        Codec codec = new Codec();
                        this.Visible = false;
                        codec.Visible = false;
                        codec.ShowDialog();
                        break;
                    }
                default:
                    break;
            }
        }

        public Menu()
        {
            InitializeComponent();
            if (Login.flag)
            {
                logins = Logins.ReadListTextOrBin(Codec.LevelOfAppEncryption, Codec.TextTrue_BinFalse);
                Start();
            }
            else
            {
                label = new Label();
                label.Location = new Point(320, 180);
                label.Size = new Size(125, 45);
                label.Text = "Zugriff abgelehnt";
                label.Font = new Font("sans-serif", 15f);
                label.AutoSize = true;
                label.Visible = true;
            }
        }
    }
}
