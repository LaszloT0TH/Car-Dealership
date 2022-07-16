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
    public partial class NewSalesperson : Form
    {
        InputForm form;

        void New_Salesperson()
        {
            form = new InputForm(this);
            form.Add("FName", (new InputField("Familienname")).AddRule("[A-ZÖÜ]{1}[a-zäöüß]{1,30}[ ]{0,1}[A-ZÖÜ]{0,1}[a-zäöüß]{0,30}[ ]{0,1}[A-ZÖÜ]{0,1}[a-zäöüß]{0,30}"))
                .Add("VName", (new InputField("Vorname")).AddRule("[A-ZÖÜ]{1}[a-zäöüß]{1,30}[ ]{0,1}[A-ZÖÜ]{0,1}[a-zäöüß]{0,30}"))
                .Add("Provision", (new InputField("Provision in %")).AddRule("[1-9][0-9]{0,1}[,]{0,1}[0-9]{0,2}"))
                .Add("Eintritt", (new InputField("Eintrittsdatum (TT.MM.JJJJ)")).AddRule("[0-9]{2}[.][0-1]{1}[0-9]{1}[.][1-9][0-9]{3}"))
                // Secret
                .Add("Lohn", (new InputField("Lohn in €")).AddRule("[1-9][0-9]{0,5}[,]{0,1}[0-9]{0,2}"))
                .Add("Geburtsdatum", (new InputField("Geburtsdatum (TT.MM.JJJJ)")).AddRule("[0-9]{2}[.][0-1]{1}[0-9]{1}[.][1-9][0-9]{3}"))
                .Add("PLZ", (new InputField("Postleitzahl")).AddRule("[1-9][0-9]{3}"))
                .Add("Ort", (new InputField("Ort")).AddRule("[A-ZÖÜ][a-zäöüß]{1,30}[ ]{0,1}[-]{0,1}[A-ZÖÜ]{0,1}[a-zäöüß]{0,30}[ ]{0,1}[-]{0,1}[A-ZÖÜ]{0,1}[a-zäöüß]{0,30}"))
                .Add("Strasse", (new InputField("Adresse: Strasse Hausnummer/Stiege/Türnummer")).AddRule("[A-ZÖÜ]{1}[a-zäöüß]{1,30}[ ]{0,1}[-]{0,1}[A-ZÖÜ]{0,1}[a-zäöüß]{0,30}[ ]{0,1}[-]{0,1}[A-ZÖÜ]{0,1}[a-zäöüß]{0,30}[ ]{1}[1-9]{1}[0-9]{0,2}[-]{0,1}[1-9]{0,1}[0-9]{0,2}[/]{0,1}[1-9]{0,1}[/]{0,1}[1-9]{0,1}[0-9]{0,2}[-]{0,1}[1-9]{0,1}[0-9]{0,2}"))
                .MoveTo(10, 10)
                .SetButtonZero("Send")
                .SetButtonFirst("Kündigung", Visible = true)
                .SetButtonSecond("Menu", Visible = true)
                .SetButtonThird()
                .OnSubmitZero(() =>
                {
                    WriteNewSalesPerson();

                    WriteNewSalespersonSecretData();

                    MessageBox.Show($"Geschpeichert\n\n{form["FName"]} {form["VName"]} ({form["Eintritt"]})");

                    InputField.ClearTextBox(this);
                })
                .OnSubmitFirst(() =>
                {
                    SalespersonsList salespersonsList = new SalespersonsList();
                    this.Visible = false;
                    salespersonsList.Visible = false;
                    salespersonsList.ShowDialog();
                })
                .OnSubmitSecond(() =>
                {
                    Menu menu = new Menu();
                    this.Visible = false;
                    menu.Visible = false;
                    menu.ShowDialog();
                });
        }

        private void WriteNewSalesPerson()
        {
            string lastName = form["FName"];
            string firstName = form["VName"];
            double provision = Convert.ToDouble(form["Provision"]);
            DateTime entry = Convert.ToDateTime(form["Eintritt"]);
            SalesPersons.SalesPerson salesPerson = new SalesPersons.SalesPerson(lastName, firstName, provision, entry);
            SalesPersons.WriteAddTextOrBin(salesPerson, Codec.LevelOfAppEncryption, Codec.TextTrue_BinFalse);
        }

        private void WriteNewSalespersonSecretData()
        {
            double wage = Convert.ToDouble(form["Lohn"]);
            DateTime date_of_birth = Convert.ToDateTime(form["Geburtsdatum"]);
            int postalCode = Convert.ToInt32(form["PLZ"]);
            string location = form["Ort"];
            string street = form["Strasse"];
            SalesPersons_Secret_Data.Salesperson_Secret_Data salesperson_Secret_Data = new SalesPersons_Secret_Data.Salesperson_Secret_Data(wage, date_of_birth, postalCode, location, street);
            SalesPersons_Secret_Data.WriteAddTextOrBin(salesperson_Secret_Data, Codec.LevelOfAppEncryption, Codec.TextTrue_BinFalse);
        }

        public NewSalesperson()
        {
            InitializeComponent();
            New_Salesperson();
        }
    }
}
