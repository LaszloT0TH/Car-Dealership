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
    public partial class CostumersList : Form
    {
        public static int costId = 0;

        InputForm form;

        NewSale newSale;

        List<Costumers> costumers;

        string[] costumersArray;

        void Costumers_List()
        {
            form = new InputForm(this);
            form.Add("Costumers", new InputSelect("Kunden List:", costumersArray).SetSize(800))

                .MoveTo(10, 10)
                .SetButtonZero("Weiter")
                .SetButtonFirst("Menu", Visible = true)
                .SetButtonSecond()
                .SetButtonThird()
                .OnSubmitZero(() =>
                {
                    string[] tempCostId = form["Costumers"].Split(';');
                    costId = Convert.ToInt32(tempCostId[0]);

                    MessageBox.Show(form["Costumers"]);

                    newSale = new NewSale();
                    this.Visible = false;
                    newSale.Visible = false;
                    newSale.ShowDialog();
                })
                .OnSubmitFirst(() =>
                {
                    Menu menu = new Menu();
                    this.Visible = false;
                    menu.Visible = false;
                    menu.ShowDialog();
                });
        }

        private void CreateSelectList()
        {
            costumersArray = new string[costumers.Count];
            for (int i = 0; i < costumers.Count; i++)
            {
                costumersArray[i] += costumers[i].CostumerId + ";" + costumers[i].LastName
                    + ";" + costumers[i].Street + ";" + costumers[i].Location + "; +" + costumers[i].TelNr;
            }
        }

        public CostumersList()
        {
            InitializeComponent();
            costumers = Costumers.ReadListTextOrBin(Codec.LevelOfAppEncryption, Codec.TextTrue_BinFalse);
            CreateSelectList();
            Costumers_List();
        }
    }
}
