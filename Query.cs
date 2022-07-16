using InputForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Car_Dealership
{
    public partial class Query : Form
    {
        InputForm form;

        Label label;

        List<Cars> cars;

        List<Costumers> costumers;

        List<Sales> sales;

        List<SalesPersons> salespersons;

        List<SalesPersons_Secret_Data> salespersons_Secret_Datas;

        List<Usernames_And_Passwords> usernames_And_Passwords;

        const string DropDownMenu1 = "Alle Verkäufe sortiert nach Datum abstiegend";

        const string DropDownMenu2 = "Kunden aus Wien die noch nicht gekauft haben sortiert nach Familiename aufstiegend";

        const string DropDownMenu3 = "Verkäufer die noch nicht verkauft haben sortiert nach Eintrittsdatum aufstiegend";

        const string DropDownMenu4 = "Alle Kunden gruppiert nach PLZ aufstiegend, Familienname und Vorname abstiegend";

        const string DropDownMenu5 = "Die besten drei Verkäufer sortiert nach Verkaufmenge abstiegend";

        const string DropDownMenu6 = "Verkäufer Daten";

        const string Help = "Die Anwendung speichert die Abfragen. " +
                       "\nDas fertige Dokument finden Sie in der Anwendungsordner mit Titel: Abfrage Datum und Uhrzeit";

        private void Start()
        {
            form = new InputForm(this);
            form.Add("Query", new InputSelect("Abfrage:", new string[] {
                DropDownMenu1,
                DropDownMenu2,
                DropDownMenu3,
                DropDownMenu4,
                DropDownMenu5,
                DropDownMenu6
            }).SetSize(800))
                .MoveTo(10, 10)
                .SetButtonZero("OK")
                .SetButtonFirst("Menü", Visible = true)
                .SetButtonSecond("Info", Visible = true)
                .SetButtonThird()
                .OnSubmitZero(() =>
                {
                    switch (form["Query"])
                    {
                        case DropDownMenu1:
                            form.Visible = false;
                            InnerJoin();
                            break;
                        case DropDownMenu2:
                            form.Visible = false;
                            LeftJoinCostumers();
                            break;
                        case DropDownMenu3:
                            form.Visible = false;
                            LeftJoinSalespersons();
                            break;
                        case DropDownMenu4:
                            form.Visible = false;
                            GroupByCostumer();
                            break;
                        case DropDownMenu5:
                            form.Visible = false;
                            TheBestThreeSalesPerson();
                            break;
                        case DropDownMenu6:
                            form.Visible = false;
                            SalesPersonData();
                            break;
                        default:
                            break;
                    }
                })
                .OnSubmitFirst(() =>
                {
                    Menu menu = new Menu();
                    this.Visible = false;
                    menu.Visible = false;
                    menu.ShowDialog();
                })
                .OnSubmitSecond(() =>
                {
                    label.Text = Help;
                });
        }

        private void InnerJoin()
        {
            label.Visible = false;

            var query = from cost in costumers
                        join sale in sales
                        on cost.CostumerId equals sale._costumerId
                        join car in cars
                        on sale._carNumber equals car.CarNumber
                        join salesp in salespersons
                        on sale._salesId equals salesp._salesId
                        join salesp_secret in salespersons_Secret_Datas
                        on salesp._salesId equals salesp_secret._salesId
                        orderby sale._date descending
                        select new
                        {
                            costCostumerId = cost.CostumerId,
                            costSex = cost.Sex,
                            costLastName = cost.LastName,
                            costFirstName = cost.FirstName,
                            costStreet = cost.Street,
                            costPostalCcode = cost.PostalCcode,
                            costLocation = cost.Location,
                            costCountry = cost.Country,
                            costTelnr = cost.TelNr,
                            costEmail = cost.Email,

                            carCarNumber = car.CarNumber,
                            carModel = car.Model,
                            carColor = car.Color,
                            carNumber_of_seats = car.Number_of_seats,
                            carCubic_capacity = car.Cubic_capacity,
                            carMileage = car.Mileage,
                            carYear_of_production = car.Year_of_production,
                            carChassis_number = car.Chassis_number,
                            carEngine_power = car.Engine_power,
                            carGearbox = car.Gearbox,
                            carFuel = car.Fuel,
                            carOwn_Weight = car.Own_Weight,

                            salesp_salesId = salesp._salesId,
                            salesp_lastName = salesp._lastName,
                            salesp_firstName = salesp._firstName,
                            salesp_provision = salesp._provision,
                            salesp_entry = salesp._entry,
                            salesp_active = salesp._active,

                            salesp_secret_wage = salesp_secret._wage,
                            salesp_secret_date_of_birth = salesp_secret._date_of_birth,
                            salesp_secret_postalCcode = salesp_secret._postalCcode,
                            salesp_secret_location = salesp_secret._location,
                            salesp_secret_street = salesp_secret._street,

                            sale_date = sale._date
                        };
            object[] rows = new object[query.ToList().Count];
            int counter = 0;
            foreach (var q in query)
            {
                string[] row = new string[]
                {
                    Convert.ToString(q.costCostumerId),
                    q.costSex,
                    q.costLastName,
                    q.costFirstName,
                    q.costStreet,
                    Convert.ToString(q.costPostalCcode),
                    q.costLocation,
                    Convert.ToString(q.costCountry),
                    Convert.ToString(q.costTelnr),
                    q.costEmail,
                    Convert.ToString(q.carCarNumber),
                    q.carModel,
                    q.carColor,
                    Convert.ToString(q.carNumber_of_seats),
                    Convert.ToString(q.carCubic_capacity),
                    Convert.ToString(q.carMileage),
                    Convert.ToString(q.carYear_of_production),
                    q.carChassis_number,
                    Convert.ToString(q.carEngine_power),
                    Convert.ToString(q.carGearbox),
                    q.carFuel,
                    Convert.ToString(q.carOwn_Weight),
                    Convert.ToString(q.salesp_salesId),
                    q.salesp_lastName,
                    q.salesp_firstName,
                    Convert.ToString(q.salesp_provision),
                    Convert.ToString(q.salesp_entry),
                    Convert.ToString(q.salesp_active),
                    Convert.ToString(q.salesp_secret_wage),
                    Convert.ToString(q.salesp_secret_date_of_birth),
                    Convert.ToString(q.salesp_secret_postalCcode),
                    q.salesp_secret_location,
                    q.salesp_secret_street,
                    Convert.ToString(q.sale_date)
                };
                rows[counter++] = row;
            }
            string costCostumerId = "Kunde Nummer";
            string costSex = "Kunde Geschlecht";
            string costLastName = "Kunde Familiename";
            string costFirstName = "Kunde Vorname";
            string costStreet = "Kunde Adresse";
            string costPostalCcode = "Kunde Postleitzahl";
            string costLocation = "Kunde WohnOrt";
            string costCountry = "Kunde Land";
            string costTelnr = "Kunde Telefonnummer";
            string costEmail = "Kunde E-Mail-Adresse";

            string carCarNumber = "Auto Id";
            string carModel = "Auto Modell";
            string carColor = "Auto Farbe";
            string carNumber_of_seats = "Auto Anzahl der Sitzplätze";
            string carCubic_capacity = "Auto Hubraum cm3";
            string carMileage = "Auto Fahrleistung KM";
            string carYear_of_production = "Auto Baujahr";
            string carChassis_number = "Auto Fahrgestellnummer";
            string carEngine_power = "Auto Motorleistung KW";
            string carGearbox = "Auto Getriebe";
            string carFuel = "Auto Brennstoff";
            string carOwn_Weight = "Auto Eigengewicht kg";

            string salesp_salesId = "Verkaufer Id";
            string salesp_lastName = "Verkaufer Familienname";
            string salesp_firstName = "Verkaufer Vorname";
            string salesp_provision = "Verkaufer Provision in %";
            string salesp_entry = "Verkaufer Eintrittsdatum";
            string salesp_active = "Verkaufer Aktiv";

            string salesp_secret_wage = "Verkaufer Lohn in €";
            string salesp_secret_date_of_birth = "Verkaufer Geburtsdatum";
            string salesp_secret_postalCcode = "Verkaufer Postleitzahl";
            string salesp_secret_location = "Verkaufer WohnOrt";
            string salesp_secret_street = "Verkaufer Adresse";

            string sale_date = "Verkaufs Datum";
            form = new InputForm(this);
            form.Add("Grid", new DispalyTable(DropDownMenu1, new string[] {
                 costCostumerId,
                 costSex,
                 costLastName,
                 costFirstName,
                 costStreet,
                 costPostalCcode,
                 costLocation,
                 costCountry,
                 costTelnr,
                 costEmail,

                 carCarNumber,
                 carModel,
                 carColor,
                 carNumber_of_seats,
                 carCubic_capacity,
                 carMileage,
                 carYear_of_production,
                 carChassis_number,
                 carEngine_power,
                 carGearbox,
                 carFuel,
                 carOwn_Weight,

                 salesp_salesId,
                 salesp_lastName,
                 salesp_firstName,
                 salesp_provision,
                 salesp_entry,
                 salesp_active,

                 salesp_secret_wage,
                 salesp_secret_date_of_birth,
                 salesp_secret_postalCcode,
                 salesp_secret_location,
                 salesp_secret_street,

                 sale_date})
                .Add(rows))
                .MoveTo(10, 10)
                .SetButtonPosition(1170)
                .SetButtonZero("Speichern")
                .SetButtonFirst("Menü", Visible = true)
                .SetButtonSecond()
                .SetButtonThird()
                .OnSubmitZero(() =>
                {
                    string date = DateTime.Now.ToString().Replace(':', '-');
                    StreamWriter writer = new StreamWriter("Abfrage " + DropDownMenu1 + " " + date + ".csv");
                    // Table Rows Title
                    writer.WriteLine($"{costCostumerId}; {costSex}; {costLastName}; " +
                            $"{costFirstName}; {costStreet}; {costPostalCcode}; {costLocation}; " +
                            $"{costCountry}; {costTelnr}; {costEmail}; " +
                            $"{carCarNumber}; {carModel}; {carColor}; {carNumber_of_seats}; " +
                            $"{carCubic_capacity}; {carMileage}; {carYear_of_production}; {carChassis_number};" +
                            $"{carEngine_power}; {carGearbox}; {carFuel}; {carOwn_Weight}; " +
                            $"{salesp_salesId}; {salesp_lastName}; {salesp_firstName}; {salesp_provision}; " +
                            $"{salesp_entry}; {salesp_active}; {salesp_secret_wage}; {salesp_secret_date_of_birth}; " +
                            $"{salesp_secret_postalCcode}; {salesp_secret_location}; {salesp_secret_street}; " +
                            $"{sale_date}");
                    foreach (var q in query)
                    {
                        writer.WriteLine($"{q.costCostumerId}; {q.costSex}; {q.costLastName}; " +
                            $"{q.costFirstName}; {q.costStreet}; {q.costPostalCcode}; {q.costLocation}; " +
                            $"{q.costCountry}; {q.costTelnr}; {q.costEmail}; {q.carCarNumber}; {q.carModel};" +
                            $"{q.carColor}; {q.carNumber_of_seats}; {q.carCubic_capacity}; {q.carMileage}; " +
                            $"{q.carYear_of_production}; {q.carChassis_number}; {q.carEngine_power}; " +
                            $"{q.carGearbox}; {q.carFuel}; {q.carOwn_Weight}; " +
                            $"{q.salesp_salesId}; {q.salesp_lastName}; {q.salesp_firstName}; {q.salesp_provision}; " +
                            $"{q.salesp_entry}; {q.salesp_active}; " +
                            $"{q.salesp_secret_wage}; {q.salesp_secret_date_of_birth}; {q.salesp_secret_postalCcode}; " +
                            $"{q.salesp_secret_location}; {q.salesp_secret_street}; " +
                            $"{q.sale_date}");
                    }
                    writer.Close();
                    MessageBox.Show("Geschpeichert");
                })
                .OnSubmitFirst(() =>
                {
                    Menu menu = new Menu();
                    this.Visible = false;
                    menu.Visible = false;
                    menu.ShowDialog();
                });
        }

        private void LeftJoinCostumers()
        {
            label.Visible = false;

            var query = from cost in costumers
                        join sale in sales
                        on cost.CostumerId equals sale._costumerId into space
                        from newsale in space.DefaultIfEmpty(
                            new Sales
                            {
                                _saleId = 0
                            })
                        where newsale._saleId == 0
                        where cost.Location == "Wien"
                        orderby cost.LastName
                        select new
                        {

                            costCostumerId = cost.CostumerId,
                            costSex = cost.Sex,
                            costLastName = cost.LastName,
                            costFirstName = cost.FirstName,
                            costStreet = cost.Street,
                            costPostalCcode = cost.PostalCcode,
                            costLocation = cost.Location,
                            costCountry = cost.Country,
                            costTelnr = cost.TelNr,
                            costEmail = cost.Email,

                            newsale._saleId,
                            newsale._costumerId,
                            newsale._carNumber,
                            newsale._salesId,
                            newsale._date

                        };
            object[] rows = new object[query.ToList().Count];
            int counter = 0;
            foreach (var q in query)
            {
                string[] row = new string[]{Convert.ToString(q.costCostumerId), q.costSex, q.costLastName,
                q.costFirstName, q.costStreet, Convert.ToString(q.costPostalCcode), q.costLocation,
                    Convert.ToString(q.costCountry), Convert.ToString(q.costTelnr), q.costEmail
                };
                rows[counter++] = row;
            }
            string costCostumerId = "Kunde Nummer";
            string costSex = "Kunde Geschlecht";
            string costLastName = "Kunde Familiename";
            string costFirstName = "Kunde Vorname";
            string costStreet = "Kunde Adresse";
            string costPostalCcode = "Kunde Postleitzahl";
            string costLocation = "Kunde WohnOrt";
            string costCountry = "Kunde Land";
            string costTelnr = "Kunde Telefonnummer";
            string costEmail = "Kunde E-Mail-Adresse";
            form = new InputForm(this);
            form.Add("Grid", new DispalyTable(DropDownMenu2, new string[] {
                 costCostumerId,
                 costSex,
                 costLastName,
                 costFirstName,
                 costStreet,
                 costPostalCcode,
                 costLocation,
                 costCountry,
                 costTelnr,
                 costEmail})
                .Add(rows))
                .MoveTo(10, 10)
                .SetButtonPosition(1170)
                .SetButtonZero("Speichern")
                .SetButtonFirst("Menü", Visible = true)
                .SetButtonSecond()
                .SetButtonThird()
                .OnSubmitZero(() =>
                {
                    string date = DateTime.Now.ToString().Replace(':', '-');
                    StreamWriter writer = new StreamWriter("Abfrage " + DropDownMenu2 + " " + date + ".csv");
                    // Table Rows Title
                    writer.WriteLine($"{costCostumerId}; {costSex}; {costLastName}; " +
                            $"{costFirstName}; {costStreet}; {costPostalCcode}; {costLocation}; " +
                            $"{costCountry}; {costTelnr}; {costEmail}");
                    foreach (var q in query)
                    {
                        writer.WriteLine($"{q.costCostumerId}; {q.costSex}; {q.costLastName}; " +
                            $"{q.costFirstName}; {q.costStreet}; {q.costPostalCcode}; {q.costLocation}; " +
                            $"{q.costCountry}; {q.costTelnr}; {q.costEmail}");
                    }
                    writer.Close();
                    MessageBox.Show("Geschpeichert");
                })
                .OnSubmitFirst(() =>
                {
                    Menu menu = new Menu();
                    this.Visible = false;
                    menu.Visible = false;
                    menu.ShowDialog();
                });
        }

        private void LeftJoinSalespersons()
        {
            label.Visible = false;

            var query = from salesp in salespersons
                        join salesp_secret in salespersons_Secret_Datas
                        on salesp._salesId equals salesp_secret._salesId
                        join sale in sales
                        on salesp._salesId equals sale._salesId
                        into space
                        from newsale in space.DefaultIfEmpty(
                            new Sales
                            {
                                _carNumber = 0
                            })
                        where newsale._carNumber == 0
                        orderby salesp._entry
                        select new
                        {

                            salesp_salesId = salesp._salesId,
                            salesp_lastName = salesp._lastName,
                            salesp_firstName = salesp._firstName,
                            salesp_provision = salesp._provision,
                            salesp_entry = salesp._entry,
                            salesp_active = salesp._active,

                            salesp_secret_wage = salesp_secret._wage,
                            salesp_secret_date_of_birth = salesp_secret._date_of_birth,
                            salesp_secret_postalCcode = salesp_secret._postalCcode,
                            salesp_secret_location = salesp_secret._location,
                            salesp_secret_street = salesp_secret._street,

                            newsale._saleId,
                            newsale._costumerId,
                            newsale._carNumber,
                            newsale._salesId,
                            newsale._date

                        };
            object[] rows = new object[query.ToList().Count];
            int counter = 0;
            foreach (var q in query)
            {
                string[] row = new string[]{Convert.ToString(q.salesp_salesId), q.salesp_lastName, q.salesp_firstName,
                    Convert.ToString(q.salesp_provision),Convert.ToString(q.salesp_entry),Convert.ToString(q.salesp_active),
                    Convert.ToString(q.salesp_secret_wage), Convert.ToString(q.salesp_secret_date_of_birth),
                    Convert.ToString(q.salesp_secret_postalCcode), q.salesp_secret_location,  q.salesp_secret_street
                };
                rows[counter++] = row;
            }
            string salesp_salesId = "Verkaufer Id";
            string salesp_lastName = "Verkaufer Familienname";
            string salesp_firstName = "Verkaufer Vorname";
            string salesp_provision = "Verkaufer Provision in %";
            string salesp_entry = "Verkaufer Eintrittsdatum";
            string salesp_active = "Verkaufer Aktiv";

            string salesp_secret_wage = "Verkaufer Lohn in €";
            string salesp_secret_date_of_birth = "Verkaufer Geburtsdatum";
            string salesp_secret_postalCcode = "Verkaufer Postleitzahl";
            string salesp_secret_location = "Verkaufer WohnOrt";
            string salesp_secret_street = "Verkaufer Adresse";
            form = new InputForm(this);
            form.Add("Grid", new DispalyTable(DropDownMenu3, new string[] {
                 salesp_salesId,
                 salesp_lastName,
                 salesp_firstName,
                 salesp_provision,
                 salesp_entry,
                 salesp_active,

                 salesp_secret_wage,
                 salesp_secret_date_of_birth,
                 salesp_secret_postalCcode,
                 salesp_secret_location,
                 salesp_secret_street
            })
                .Add(rows))
                .MoveTo(10, 10)
                .SetButtonPosition(1170)
                .SetButtonZero("Speichern")
                .SetButtonFirst("Menü", Visible = true)
                .SetButtonSecond()
                .SetButtonThird()
                .OnSubmitZero(() =>
                {
                    string date = DateTime.Now.ToString().Replace(':', '-');
                    StreamWriter writer = new StreamWriter("Abfrage " + DropDownMenu3 + " " + date + ".csv");
                    // Table Rows Title
                    writer.WriteLine($"{salesp_salesId}; {salesp_lastName}; {salesp_firstName}; {salesp_provision}; " +
                            $"{salesp_entry}; {salesp_active}; {salesp_secret_wage}; {salesp_secret_date_of_birth}; " +
                            $"{salesp_secret_postalCcode}; {salesp_secret_location}; {salesp_secret_street}");
                    foreach (var q in query)
                    {
                        writer.WriteLine($"{q.salesp_salesId}; {q.salesp_lastName}; {q.salesp_firstName}; {q.salesp_provision}; " +
                            $"{q.salesp_entry}; {q.salesp_active}; " +
                            $"{q.salesp_secret_wage}; {q.salesp_secret_date_of_birth}; {q.salesp_secret_postalCcode}; " +
                            $"{q.salesp_secret_location}; {q.salesp_secret_street}");
                    }
                    writer.Close();
                    MessageBox.Show("Geschpeichert");
                })
                .OnSubmitFirst(() =>
                {
                    Menu menu = new Menu();
                    this.Visible = false;
                    menu.Visible = false;
                    menu.ShowDialog();
                });
        }

        private void GroupByCostumer()
        {
            label.Visible = false;

            var query = from cost in costumers
                        group cost by cost.PostalCcode
                        into eGroup
                        orderby eGroup.Key ascending
                        select new
                        {
                            Key = eGroup.Key,
                            Costumers = eGroup.OrderBy(x => x.LastName)
                                              .ThenBy(x => x.FirstName)
                        };
            int exterior = 0, interior = 0;
            foreach (var group in query)
            {
                exterior++;
                foreach (var q in group.Costumers) { interior++; }
            }
            object[] rows = new object[exterior + interior];
            int counter = 0;
            foreach (var group in query)
            {
                string[] rowEx = new string[] { "PLZ " + group.Key.ToString() };
                rows[counter++] = rowEx;
                foreach (var q in group.Costumers)
                {
                    string[] row = new string[]{ q.LastName, q.FirstName, q.Sex, q.Street, q.Location,
                    Convert.ToString(q.Country), Convert.ToString(q.TelNr), q.Email };
                    rows[counter++] = row;
                }
            }

            string costLastName = "Kunde Familiename";
            string costFirstName = "Kunde Vorname";
            string costSex = "Kunde Geschlecht";
            string costStreet = "Kunde Adresse";
            string costLocation = "Kunde WohnOrt";
            string costCountry = "Kunde Land";
            string costTelnr = "Kunde Telefonnummer";
            string costEmail = "Kunde E-Mail-Adresse";

            form = new InputForm(this);
            form.Add("Grid", new DispalyTable(DropDownMenu4, new string[] {
                 costLastName,
                 costFirstName,
                 costSex,
                 costStreet,
                 costLocation,
                 costCountry,
                 costTelnr,
                 costEmail})
                .Add(rows))
                .MoveTo(10, 10)
                .SetButtonPosition(1170)
                .SetButtonZero("Speichern")
                .SetButtonFirst("Menü", Visible = true)
                .SetButtonSecond()
                .SetButtonThird()
                .OnSubmitZero(() =>
                {
                    string date = DateTime.Now.ToString().Replace(':', '-');
                    StreamWriter writer = new StreamWriter("Abfrage " + DropDownMenu4 + " " + date + ".csv");
                    // Table Rows Title
                    writer.WriteLine($" {costLastName}; {costFirstName}; {costSex};" +
                            $"{costStreet}; {costLocation};{costCountry}; {costTelnr}; {costEmail}");
                    foreach (var group in query)
                    {
                        writer.WriteLine($"{"PLZ " + group.Key.ToString()}");
                        foreach (var q in group.Costumers)
                        {
                            writer.WriteLine($"{q.LastName};{q.FirstName};{q.Sex};" +
                                $" {q.Street}; {q.Location}; {q.Country}; {q.TelNr}; {q.Email}");
                        }
                    }
                    writer.Close();
                    MessageBox.Show("Geschpeichert");
                })
                .OnSubmitFirst(() =>
                {
                    Menu menu = new Menu();
                    this.Visible = false;
                    menu.Visible = false;
                    menu.ShowDialog();
                });
        }

        private void TheBestThreeSalesPerson()
        {
            label.Visible = false;

            var queryhelp = from sale in sales
                            group sale by sale._salesId
                            into eGroup
                            orderby eGroup.Key ascending
                            select new
                            {
                                Key = eGroup.Key,
                                Salesperson = eGroup.OrderBy(x => x._salesId)

                            };
            List<myHelpList> help = new List<myHelpList>();

            foreach (var group in queryhelp)
            {
                help.Add(new myHelpList() { Key = group.Key, Count = group.Salesperson.Count() });
            }

            var query = (from hl in help
                         join salesp in salespersons
                         on hl.Key equals salesp._salesId
                         join salesp_secret in salespersons_Secret_Datas
                         on salesp._salesId equals salesp_secret._salesId
                         orderby hl.Count descending
                         select new
                         {
                             hlCount = hl.Count,
                             hlKey = hl.Key,

                             salesp_salesId = salesp._salesId,
                             salesp_lastName = salesp._lastName,
                             salesp_firstName = salesp._firstName,
                             salesp_provision = salesp._provision,
                             salesp_entry = salesp._entry,
                             salesp_active = salesp._active,

                             salesp_secret_wage = salesp_secret._wage,
                             salesp_secret_date_of_birth = salesp_secret._date_of_birth,
                             salesp_secret_postalCcode = salesp_secret._postalCcode,
                             salesp_secret_location = salesp_secret._location,
                             salesp_secret_street = salesp_secret._street
                         }).Take(3);


            object[] rows = new object[query.ToList().Count];
            int counter = 0;
            foreach (var q in query)
            {
                string[] row = new string[]{
                    Convert.ToString(q.hlCount),q.salesp_lastName, q.salesp_firstName,
                    Convert.ToString(q.salesp_provision), Convert.ToString(q.salesp_entry), Convert.ToString(q.salesp_secret_wage),
                    Convert.ToString(q.salesp_secret_date_of_birth), Convert.ToString(q.salesp_secret_postalCcode),
                    q.salesp_secret_location, q.salesp_secret_street
                };
                rows[counter++] = row;
            }

            string Verkaufmenge = "Verkaufmenge";

            string salesp_lastName = "Verkaufer Familienname";
            string salesp_firstName = "Verkaufer Vorname";
            string salesp_provision = "Verkaufer Provision in %";
            string salesp_entry = "Verkaufer Eintrittsdatum";

            string salesp_secret_wage = "Verkaufer Lohn in €";
            string salesp_secret_date_of_birth = "Verkaufer Geburtsdatum";
            string salesp_secret_postalCcode = "Verkaufer Postleitzahl";
            string salesp_secret_location = "Verkaufer WohnOrt";
            string salesp_secret_street = "Verkaufer Adresse";

            form = new InputForm(this);
            form.Add("Grid", new DispalyTable(DropDownMenu5, new string[] {

                 Verkaufmenge,
                 salesp_lastName,
                 salesp_firstName,
                 salesp_provision,
                 salesp_entry,

                 salesp_secret_wage,
                 salesp_secret_date_of_birth,
                 salesp_secret_postalCcode,
                 salesp_secret_location,
                 salesp_secret_street})
                .Add(rows))
                .MoveTo(10, 10)
                .SetButtonPosition(1170)
                .SetButtonZero("Speichern")
                .SetButtonFirst("Menü", Visible = true)
                .SetButtonSecond()
                .SetButtonThird()
                .OnSubmitZero(() =>
                {
                    string date = DateTime.Now.ToString().Replace(':', '-');
                    StreamWriter writer = new StreamWriter("Abfrage " + DropDownMenu5 + " " + date + ".csv");
                    // Table Rows Title
                    writer.WriteLine($"{Verkaufmenge}; {salesp_lastName}; {salesp_firstName}; {salesp_provision}; " +
                            $"{salesp_entry}; {salesp_secret_wage}; {salesp_secret_date_of_birth}; " +
                            $"{salesp_secret_postalCcode}; {salesp_secret_location}; {salesp_secret_street}");
                    foreach (var q in query)
                    {
                        writer.WriteLine($"{q.hlCount};{q.salesp_lastName}; {q.salesp_firstName}; " +
                            $"{q.salesp_provision}; {q.salesp_entry}; {q.salesp_secret_wage}; " +
                            $"{q.salesp_secret_date_of_birth}; {q.salesp_secret_postalCcode}; " +
                            $"{q.salesp_secret_location}; {q.salesp_secret_street}");
                    }
                    writer.Close();
                    MessageBox.Show("Geschpeichert");
                })
                .OnSubmitFirst(() =>
                {
                    Menu menu = new Menu();
                    this.Visible = false;
                    menu.Visible = false;
                    menu.ShowDialog();
                });
        }

        private void SalesPersonData()
        {
            label.Visible = false;

            var query = from salesp in salespersons
                        join salesp_secret in salespersons_Secret_Datas
                        on salesp._salesId equals salesp_secret._salesId
                        join spUsernameAndPassword in usernames_And_Passwords
                        on salesp_secret._salesId equals spUsernameAndPassword.passwordNumbers
                        orderby salesp._salesId
                        select new
                        {
                            salesp_salesId = salesp._salesId,
                            salesp_lastName = salesp._lastName,
                            salesp_firstName = salesp._firstName,
                            salesp_provision = salesp._provision,
                            salesp_entry = salesp._entry,
                            salesp_active = salesp._active,

                            salesp_secret_wage = salesp_secret._wage,
                            salesp_secret_date_of_birth = salesp_secret._date_of_birth,
                            salesp_secret_postalCcode = salesp_secret._postalCcode,
                            salesp_secret_location = salesp_secret._location,
                            salesp_secret_street = salesp_secret._street,

                            username = spUsernameAndPassword.usernames,
                            password = spUsernameAndPassword.passwords

                        };
            object[] rows = new object[query.ToList().Count];
            int counter = 0;
            foreach (var q in query)
            {
                string[] row = new string[]{ Convert.ToString(q.salesp_salesId), q.salesp_lastName, q.salesp_firstName,
                    Convert.ToString(q.salesp_provision), Convert.ToString(q.salesp_entry),
                    Convert.ToString(q.salesp_active), Convert.ToString(q.salesp_secret_wage),
                    Convert.ToString(q.salesp_secret_date_of_birth), Convert.ToString(q.salesp_secret_postalCcode),
                    q.salesp_secret_location, q.salesp_secret_street,
                    q.username, q.password
                };
                rows[counter++] = row;
            }

            string salesp_salesId = "Verkaufer Id";
            string salesp_lastName = "Verkaufer Familienname";
            string salesp_firstName = "Verkaufer Vorname";
            string salesp_provision = "Verkaufer Provision in %";
            string salesp_entry = "Verkaufer Eintrittsdatum";
            string salesp_active = "Verkaufer Aktiv";

            string salesp_secret_wage = "Verkaufer Lohn in €";
            string salesp_secret_date_of_birth = "Verkaufer Geburtsdatum";
            string salesp_secret_postalCcode = "Verkaufer Postleitzahl";
            string salesp_secret_location = "Verkaufer WohnOrt";
            string salesp_secret_street = "Verkaufer Adresse";

            string username = "Benutzer Name";
            string password = "Passwort";
            form = new InputForm(this);
            form.Add("Grid", new DispalyTable(DropDownMenu6, new string[] {
                 salesp_salesId,
                 salesp_lastName,
                 salesp_firstName,
                 salesp_provision,
                 salesp_entry,
                 salesp_active,

                 salesp_secret_wage,
                 salesp_secret_date_of_birth,
                 salesp_secret_postalCcode,
                 salesp_secret_location,
                 salesp_secret_street,

                 username,
                 password})
                .Add(rows))
                .MoveTo(10, 10)
                .SetButtonPosition(1170)
                .SetButtonZero("Speichern")
                .SetButtonFirst("Menü", Visible = true)
                .SetButtonSecond()
                .SetButtonThird()
                .OnSubmitZero(() =>
                {
                    string date = DateTime.Now.ToString().Replace(':', '-');
                    StreamWriter writer = new StreamWriter("Abfrage " + DropDownMenu6 + " " + date + ".csv");
                    // Table Rows Title
                    writer.WriteLine($"{salesp_salesId}; {salesp_lastName}; {salesp_firstName}; {salesp_provision}; " +
                            $"{salesp_entry}; {salesp_active}; {salesp_secret_wage}; {salesp_secret_date_of_birth}; " +
                            $"{salesp_secret_postalCcode}; {salesp_secret_location}; {salesp_secret_street}; " +
                            $"{username}; {password}");
                    foreach (var q in query)
                    {
                        writer.WriteLine($"{q.salesp_salesId}; {q.salesp_lastName}; {q.salesp_firstName}; {q.salesp_provision}; " +
                            $"{q.salesp_entry}; {q.salesp_active}; " +
                            $"{q.salesp_secret_wage}; {q.salesp_secret_date_of_birth}; {q.salesp_secret_postalCcode}; " +
                            $"{q.salesp_secret_location}; {q.salesp_secret_street}; " +
                            $"{q.username}; {q.password}");
                    }
                    writer.Close();
                    MessageBox.Show("Geschpeichert");
                })
                .OnSubmitFirst(() =>
                {
                    Menu menu = new Menu();
                    this.Visible = false;
                    menu.Visible = false;
                    menu.ShowDialog();
                });
        }

        private void CreateList()
        {
            cars = Cars.ReadListTextOrBin(Codec.LevelOfAppEncryption, Codec.TextTrue_BinFalse);

            costumers = Costumers.ReadListTextOrBin(Codec.LevelOfAppEncryption, Codec.TextTrue_BinFalse);

            sales = Sales.ReadListTextOrBin(Codec.LevelOfAppEncryption, Codec.TextTrue_BinFalse);

            salespersons = SalesPersons.ReadListTextOrBin(Codec.LevelOfAppEncryption, Codec.TextTrue_BinFalse);

            salespersons_Secret_Datas = SalesPersons_Secret_Data.ReadListTextOrBin(Codec.LevelOfAppEncryption, Codec.TextTrue_BinFalse);

            usernames_And_Passwords = Usernames_And_Passwords.ReadListTextOrBin(Codec.LevelOfAppEncryption, Codec.TextTrue_BinFalse);
        }

        private void CreateLabels()
        {
            label = new Label();

            label.Visible = true;

            label.Location = new Point(35, 240);

            label.Size = new Size(125, 45);

            label.Font = new Font("sans-serif", 11f);

            label.AutoSize = true;

            this.Controls.Add(label);
        }

        public class myHelpList
        {
            public int Key { get; set; }
            public int Count { get; set; }
        };

        public Query()
        {
            InitializeComponent();

            CreateLabels();

            CreateList();

            Start();
        }
    }
}
