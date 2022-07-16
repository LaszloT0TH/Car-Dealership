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
    public partial class NewCar : Form
    {
        InputForm form;

        Cars.Car car;

        void New_Car()
        {
            form = new InputForm(this);
            form.Add("Modell", (new InputField("Modell")).AddRule("[A-ZÖÜ0-9]{0,15}[a-zäöüß0-9]{1,30}[ ]{0,1}[A-ZÖÜ0-9]{0,1}[a-zäöüß0-9]{0,30}[ ]{0,1}[A-ZÖÜ0-9]{0,1}[a-zäöüß0-9]{0,30}"))
                .Add("Farbe", (new InputField("Farbe")).AddRule("[a-zäöüß]{1,20}"))
                .Add("Anzahl der Sitzplätze", (new InputField("Anzahl der Sitzplätze")).AddRule("[1-9]"))
                .Add("Hubraum", (new InputField("Hubraum cm3")).AddRule("[0-9]{3,4}"))
                .Add("Fahrleistung", (new InputField("Fahrleistung KM")).AddRule("[0-9]{0,7}"))
                .Add("Baujahr", (new InputField("Baujahr")).AddRule("[1-2][0-9]{1,3}"))
                .Add("Fahrgestellnummer", (new InputField("Fahrgestellnummer")).AddRule("[a-zäöüß0-9]{1,40}"))
                .Add("Motorleistung", (new InputField("Motorleistung KW")).AddRule("[0-9]{1,3}"))
                .Add("Getriebe", new InputSelect("Getriebe:", new string[] { "mechanisch", "halbautomatisch", "automatisch", "sequentiell" }))
                .Add("Brennstoff", new InputSelect("Brennstoff:", new string[] { "Benzin", "diesel", "erdgas", "elektrisch", "hybrid" }))
                .Add("Eigengewicht", (new InputField("Eigengewicht kg")).AddRule("[0-9]{1,4}"))
                .MoveTo(10, 10)
                .SetButtonZero("Send")
                .SetButtonFirst("Menü", Visible = true)
                .SetButtonSecond()
                .SetButtonThird()
                .OnSubmitZero(() =>
                {
                    WriteNewCar(CreateCar());

                    MessageBox.Show($"Geschpeichert\n\n{form["Modell"]} {form["Farbe"]}");

                    InputField.ClearTextBox(this);
                })
                .OnSubmitFirst(() =>
                {
                    Menu menu = new Menu();
                    this.Visible = false;
                    menu.Visible = false;
                    menu.ShowDialog();
                });
        }

        private Cars.Car CreateCar()
        {
            string model = form["Modell"];
            string color = form["Farbe"];
            int number_of_seats = Convert.ToInt32(form["Anzahl der Sitzplätze"]);
            double cubic_capacity = Convert.ToDouble(form["Hubraum"]);
            int mileage = Convert.ToInt32(form["Fahrleistung"]);
            int year_of_production = Convert.ToInt32(form["Baujahr"]);
            string chassis_number = form["Fahrgestellnummer"];
            int engine_power = Convert.ToInt32(form["Motorleistung"]);
            string gearbox = form["Getriebe"];
            string fuel = form["Brennstoff"];
            int own_Weight = Convert.ToInt32(form["Eigengewicht"]);

            car = new Cars.Car(model, color, number_of_seats, cubic_capacity,
                mileage, year_of_production, chassis_number,
                engine_power, gearbox, fuel, own_Weight);
            return car;
        }
        
        private void WriteNewCar(Cars.Car car)
        {
            Cars.WriteAddTextOrBin(car, Codec.LevelOfAppEncryption, Codec.TextTrue_BinFalse);
        }
        
        public NewCar()
        {
            InitializeComponent();
            New_Car();
        }
    }
}
