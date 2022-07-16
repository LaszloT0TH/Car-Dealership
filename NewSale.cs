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
    public partial class NewSale : Form
    {
        InputForm form;

        List<Cars> cars;

        List<Logins> logins;

        string[] carsArray;

        DateTime date = DateTime.Now;

        private void New_Sale()
        {
            form = new InputForm(this);
            form.Add("Car", new InputSelect("Auto List:", carsArray).SetSize(800))
                .MoveTo(10, 10)
                .SetButtonZero("Vekauf")
                .SetButtonFirst()
                .SetButtonSecond()
                .SetButtonThird()
                .OnSubmitZero(() =>
                {
                    WriteNewCarList();

                    WriteNewSale();

                    MessageBox.Show(form["Auto"] + " verkauft");

                    NewCostumer newCostumer = new NewCostumer();
                    this.Visible = false;
                    newCostumer.Visible = false;
                    newCostumer.ShowDialog();
                });
        }

        private void WriteNewCarList()
        {
            Cars.WriteListTextOrBin(cars, Codec.LevelOfAppEncryption, Codec.TextTrue_BinFalse, SelCarNumber());
        }

        private void WriteNewSale()
        {
            Sales.Sale sale = new Sales.Sale(CostumerId(), SalesId(), SelCarNumber(), date);

            Sales.WriteAddTextOrBin(sale, Codec.LevelOfAppEncryption, Codec.TextTrue_BinFalse);
        }

        private int CostumerId()
        {
            int costumerId = 0;

            if (NewCostumer.newcostId)
            {
                string storedId = null;
                if (File.Exists("aicostumer")) storedId = File.ReadAllText("aicostumer");
                costumerId = Convert.ToInt32(storedId);
            }
            else
            {
                costumerId = CostumersList.costId;
            }
            return costumerId; 
        }

        private int SalesId()
        {
            Logins log = logins[logins.Count - 1];

            int salesId = Convert.ToInt32(log.Password[log.Password.Length - 2].ToString() +
                log.Password[log.Password.Length - 1].ToString());

            return salesId;
        }

        private int SelCarNumber()
        {
            string[] selectedCarNumber = form["Car"].Split(';');

            int SelCarNumber = Convert.ToInt32(selectedCarNumber[0]);

            return SelCarNumber;
        }

        private void CreateSelectList()
        {
            // list of unsold cars, existing cars
            carsArray = new string[cars.Count - Cars.soldcount];
            for (int i = 0, j = 0; j < cars.Count; j++)
            {
                if (!cars[j].Sold)
                {
                    carsArray[i] += cars[j].CarNumber + ";" + cars[j].Model + ";" + cars[j].Color + ";" + cars[j].Number_of_seats
                    + ";" + cars[j].Cubic_capacity + ";" + cars[j].Mileage + ";" + cars[j].Year_of_production
                    + ";" + cars[j].Chassis_number + ";" + cars[j].Engine_power + ";" + cars[j].Gearbox + ";" + cars[j].Fuel
                    + ";" + cars[j].Own_Weight;
                    i++;
                }
            }
        }

        public NewSale()
        {
            InitializeComponent();
            cars = Cars.ReadListTextOrBin(Codec.LevelOfAppEncryption, Codec.TextTrue_BinFalse);
            CreateSelectList();
            logins = Logins.ReadListTextOrBin(Codec.LevelOfAppEncryption, Codec.TextTrue_BinFalse);
            New_Sale();
        }
    }
}
