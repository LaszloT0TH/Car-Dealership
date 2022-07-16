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
    public partial class SalespersonsList : Form
    {
        InputForm form;

        List<SalesPersons> salesPersons;

        string[] activeSalesPersonsList;

        private void Salespersons_List()
        {
            form = new InputForm(this);
            form.Add("Employee", new InputSelect("Staff List:", activeSalesPersonsList).SetSize(700))
                .MoveTo(10, 10)
                .SetButtonZero("Kündigen")
                .SetButtonFirst("Menu", Visible = true)
                .SetButtonSecond()
                .SetButtonThird()
                .OnSubmitZero(() =>
                {
                    WriteNewSalesPersonsList();

                    MessageBox.Show(form["Employee"] + " gekündigt");

                    NewSalesperson newSalesperson = new NewSalesperson();
                    this.Visible = false;
                    newSalesperson.Visible = false;
                    newSalesperson.ShowDialog();
                })
                .OnSubmitFirst(() =>
                {
                    Menu menu = new Menu();
                    this.Visible = false;
                    menu.Visible = false;
                    menu.ShowDialog();
                });
        }

        private void CreateActiveSalesPersonsList()
        {
            activeSalesPersonsList = new string[salesPersons.Count - SalesPersons.ActivePersonCounter];
            for (int i = 0, j = 0; j < salesPersons.Count; j++)
            {
                if (salesPersons[j]._active)
                {
                    activeSalesPersonsList[i] += salesPersons[j]._salesId + ";" + salesPersons[j]._lastName +
                        ";" + salesPersons[j]._firstName + ";" + salesPersons[j]._provision + ";" + salesPersons[j]._entry;
                    i++;
                }
            }
        }

        private void WriteNewSalesPersonsList()
        {
            string[] record = form["Employee"].Split(';');

            int temp_salesId = Convert.ToInt32(record[0]);

            SalesPersons.WriteListTextOrBin(salesPersons, Codec.LevelOfAppEncryption, Codec.TextTrue_BinFalse, temp_salesId);
        }

        public SalespersonsList()
        {
            InitializeComponent();

            salesPersons = SalesPersons.ReadListTextOrBin(Codec.LevelOfAppEncryption, Codec.TextTrue_BinFalse);

            CreateActiveSalesPersonsList();

            Salespersons_List();
        }
    }
}
