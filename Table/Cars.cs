using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Car_Dealership
{
    [Serializable]
    public class Cars
    {
        public static string FileName = "Car ";

        public int CarNumber { get; set; }

        public string Model { get; set; }

        public string Color { get; set; }

        public int Number_of_seats { get; set; }

        public double Cubic_capacity { get; set; }

        public int Mileage { get; set; }

        public int Year_of_production { get; set; }

        public string Chassis_number { get; set; }

        public int Engine_power { get; set; }

        public string Gearbox { get; set; }

        public string Fuel { get; set; }

        public int Own_Weight { get; set; }

        public bool Sold { get; set; }

        public static int soldcount { get; private set; }

        // Writing user input Creating a new record
        public static void WriteAddTextOrBin(Car car, int LevelOfAppEncryption, bool TextTrue_BinFalse)
        {
            if (TextTrue_BinFalse)
            {
                StreamWriter writer = new StreamWriter(FileName + LevelOfAppEncryption + " level.csv", true);
                string line =
                $"{car.Number.Encrypt(LevelOfAppEncryption)};" +
                $"{car.Model.Encrypt(LevelOfAppEncryption)};" +
                $"{car.Color.Encrypt(LevelOfAppEncryption)};" +
                $"{Convert.ToString(car.Number_of_seats).Encrypt(LevelOfAppEncryption)};" +
                $"{Convert.ToString(car.Cubic_capacity).Encrypt(LevelOfAppEncryption)};" +
                $"{Convert.ToString(car.Mileage).Encrypt(LevelOfAppEncryption)};" +
                $"{Convert.ToString(car.Year_of_production).Encrypt(LevelOfAppEncryption)};" +
                $"{car.Chassis_number.Encrypt(LevelOfAppEncryption)};" +
                $"{Convert.ToString(car.Engine_power).Encrypt(LevelOfAppEncryption)};" +
                $"{car.Gearbox.Encrypt(LevelOfAppEncryption)};" +
                $"{car.Fuel.Encrypt(LevelOfAppEncryption)};" +
                $"{Convert.ToString(car.Own_Weight).Encrypt(LevelOfAppEncryption)};" +
                $"{Convert.ToString(car.Sold).Encrypt(LevelOfAppEncryption)}";
                writer.WriteLine(line);
                writer.Close();
            }
            else
            {
                FileStream stream = new FileStream(FileName + LevelOfAppEncryption + " level.dat", FileMode.Append, FileAccess.Write);
                BinaryFormatter formatter = new BinaryFormatter();
                CarsBinary carsBinary = new CarsBinary()
                {
                    CarNumber = Convert.ToString(car.Number).Encrypt(LevelOfAppEncryption),
                    Model = Convert.ToString(car.Model).Encrypt(LevelOfAppEncryption),
                    Color = Convert.ToString(car.Color).Encrypt(LevelOfAppEncryption),
                    Number_of_seats = Convert.ToString(car.Number_of_seats).Encrypt(LevelOfAppEncryption),
                    Cubic_capacity = Convert.ToString(car.Cubic_capacity).Encrypt(LevelOfAppEncryption),
                    Mileage = Convert.ToString(car.Mileage).Encrypt(LevelOfAppEncryption),
                    Year_of_production = Convert.ToString(car.Year_of_production).Encrypt(LevelOfAppEncryption),
                    Chassis_number = Convert.ToString(car.Chassis_number).Encrypt(LevelOfAppEncryption),
                    Engine_power = Convert.ToString(car.Engine_power).Encrypt(LevelOfAppEncryption),
                    Gearbox = Convert.ToString(car.Gearbox).Encrypt(LevelOfAppEncryption),
                    Fuel = Convert.ToString(car.Fuel).Encrypt(LevelOfAppEncryption),
                    Own_Weight = Convert.ToString(car.Own_Weight).Encrypt(LevelOfAppEncryption),
                    Sold = Convert.ToString(car.Sold).Encrypt(LevelOfAppEncryption)
                };
                formatter.Serialize(stream, carsBinary);
                stream.Close();
            }
        }

        // Reading List 
        public static List<Cars> ReadListTextOrBin(int LevelOfAppEncryption, bool TextTrue_BinFalse)
        {
            if (TextTrue_BinFalse)
            {
                return ReadFromText(LevelOfAppEncryption);
            }

            else
            {
                return ReadFromBin(LevelOfAppEncryption);
            }
        }

        // Writing from List to TextList
        static void WriteListToText(List<Cars> cars, int LevelOfEncrypt, object int32SelCarNumber)
        {
            StreamWriter writer = new StreamWriter(FileName + LevelOfEncrypt + " level.csv");

            foreach (Cars car in cars)
            {
                if (int32SelCarNumber != null)
                {
                    if (car.CarNumber == (int)int32SelCarNumber)
                    {
                        car.Sold = true;
                    }
                }
                string line1 =
                    $"{Convert.ToString(car.CarNumber).Encrypt(LevelOfEncrypt)};" +
                    $"{car.Model.Encrypt(LevelOfEncrypt)};" +
                    $"{car.Color.Encrypt(LevelOfEncrypt)};" +
                    $"{Convert.ToString(car.Number_of_seats).Encrypt(LevelOfEncrypt)};" +
                    $"{Convert.ToString(car.Cubic_capacity).Encrypt(LevelOfEncrypt)};" +
                    $"{Convert.ToString(car.Mileage).Encrypt(LevelOfEncrypt)};" +
                    $"{Convert.ToString(car.Year_of_production).Encrypt(LevelOfEncrypt)};" +
                    $"{car.Chassis_number.Encrypt(LevelOfEncrypt)};" +
                    $"{Convert.ToString(car.Engine_power).Encrypt(LevelOfEncrypt)};" +
                    $"{car.Gearbox.Encrypt(LevelOfEncrypt)};" +
                    $"{car.Fuel.Encrypt(LevelOfEncrypt)};" +
                    $"{Convert.ToString(car.Own_Weight).Encrypt(LevelOfEncrypt)};" +
                    $"{Convert.ToString(car.Sold).Encrypt(LevelOfEncrypt)}";
                writer.WriteLine(line1);
            }
            writer.Close();
        }

        // Writing from List to BinaryList
        static void WriteListToBin(List<Cars> cars, int LevelOfEncrypt, object int32SelCarNumber)
        {
            FileStream stream = new FileStream(FileName + LevelOfEncrypt + " level.dat", FileMode.Append, FileAccess.Write);

            BinaryFormatter formatter = new BinaryFormatter();

            foreach (Cars car in cars)
            {
                if (int32SelCarNumber != null)
                {
                    if (car.CarNumber == (int)int32SelCarNumber)
                    {
                        car.Sold = true;
                    }
                }

                CarsBinary carsBinary = new CarsBinary()
                {
                    CarNumber = Convert.ToString(car.CarNumber).Encrypt(LevelOfEncrypt),
                    Model = Convert.ToString(car.Model).Encrypt(LevelOfEncrypt),
                    Color = Convert.ToString(car.Color).Encrypt(LevelOfEncrypt),
                    Number_of_seats = Convert.ToString(car.Number_of_seats).Encrypt(LevelOfEncrypt),
                    Cubic_capacity = Convert.ToString(car.Cubic_capacity).Encrypt(LevelOfEncrypt),
                    Mileage = Convert.ToString(car.Mileage).Encrypt(LevelOfEncrypt),
                    Year_of_production = Convert.ToString(car.Year_of_production).Encrypt(LevelOfEncrypt),
                    Chassis_number = Convert.ToString(car.Chassis_number).Encrypt(LevelOfEncrypt),
                    Engine_power = Convert.ToString(car.Engine_power).Encrypt(LevelOfEncrypt),
                    Gearbox = Convert.ToString(car.Gearbox).Encrypt(LevelOfEncrypt),
                    Fuel = Convert.ToString(car.Fuel).Encrypt(LevelOfEncrypt),
                    Own_Weight = Convert.ToString(car.Own_Weight).Encrypt(LevelOfEncrypt),
                    Sold = Convert.ToString(car.Sold).Encrypt(LevelOfEncrypt)
                };
                formatter.Serialize(stream, carsBinary);
            }
            stream.Close();
        }

        public static void WriteListTextOrBin(List<Cars> cars, int LevelOfAppEncryption, bool TextTrue_BinFalse, object int32SelCarNumber = null)
        {
            if (TextTrue_BinFalse)
            {
                WriteListToText(cars, LevelOfAppEncryption, int32SelCarNumber);
            }

            else
            {
                WriteListToBin(cars, LevelOfAppEncryption, int32SelCarNumber);
            }
        }



        static List<Cars> ReadFromText(int LevelOfDecrypt)
        {
            List<Cars> cars = new List<Cars>();

            if (File.Exists(FileName + LevelOfDecrypt + " level.csv"))
            {
                StreamReader reader = new StreamReader(FileName + LevelOfDecrypt + " level.csv", Encoding.Default);
                string line = reader.ReadLine();
                while (line != null)
                {
                    string[] record = line.Split(';');
                    int carNumber = Convert.ToInt32(record[0].Decrypt(LevelOfDecrypt));
                    string model = record[1].Decrypt(LevelOfDecrypt);
                    string color = record[2].Decrypt(LevelOfDecrypt);
                    int number_of_seats = Convert.ToInt32(record[3].Decrypt(LevelOfDecrypt));
                    double cubic_capacity = Convert.ToDouble(record[4].Decrypt(LevelOfDecrypt));
                    int mileage = Convert.ToInt32(record[5].Decrypt(LevelOfDecrypt));
                    int year_of_production = Convert.ToInt32(record[6].Decrypt(LevelOfDecrypt));
                    string chassis_number = record[7].Decrypt(LevelOfDecrypt);
                    int engine_power = Convert.ToInt32(record[8].Decrypt(LevelOfDecrypt));
                    string gearbox = record[9].Decrypt(LevelOfDecrypt);
                    string fuel = record[10].Decrypt(LevelOfDecrypt);
                    int own_Weight = Convert.ToInt32(record[11].Decrypt(LevelOfDecrypt));
                    bool sold = Convert.ToBoolean(record[12].Decrypt(LevelOfDecrypt));
                    if (sold)
                    {
                        soldcount++;
                    }
                    cars.Add(new Cars
                    {
                        CarNumber = carNumber,
                        Model = model,
                        Color = color,
                        Number_of_seats = number_of_seats,
                        Cubic_capacity = cubic_capacity,
                        Mileage = mileage,
                        Year_of_production = year_of_production,
                        Chassis_number = chassis_number,
                        Engine_power = engine_power,
                        Gearbox = gearbox,
                        Fuel = fuel,
                        Own_Weight = own_Weight,
                        Sold = sold
                    });
                    line = reader.ReadLine();
                }
                reader.Close();
            }

            return cars;
        }

        static List<Cars> ReadFromBin(int LevelOfDecrypt)
        {
            List<Cars> carsDecrypt = new List<Cars>();
            if (File.Exists(FileName + LevelOfDecrypt + " level.dat"))
            {
                FileStream stream = new FileStream(FileName + LevelOfDecrypt + " level.dat", FileMode.Open, FileAccess.Read);
                BinaryFormatter formatter = new BinaryFormatter();
                while (stream.Position != stream.Length)
                {
                    CarsBinary carBinary = (CarsBinary)formatter.Deserialize(stream);

                    int carNumber = Convert.ToInt32(carBinary.CarNumber.Decrypt(LevelOfDecrypt));
                    string model = carBinary.Model.Decrypt(LevelOfDecrypt);
                    string color = carBinary.Color.Decrypt(LevelOfDecrypt);
                    int number_of_seats = Convert.ToInt32(carBinary.Number_of_seats.Decrypt(LevelOfDecrypt));
                    double cubic_capacity = Convert.ToDouble(carBinary.Cubic_capacity.Decrypt(LevelOfDecrypt));
                    int mileage = Convert.ToInt32(carBinary.Mileage.Decrypt(LevelOfDecrypt));
                    int year_of_production = Convert.ToInt32(carBinary.Year_of_production.Decrypt(LevelOfDecrypt));
                    string chassis_number = carBinary.Chassis_number.Decrypt(LevelOfDecrypt);
                    int engine_power = Convert.ToInt32(carBinary.Engine_power.Decrypt(LevelOfDecrypt));
                    string gearbox = carBinary.Gearbox.Decrypt(LevelOfDecrypt);
                    string fuel = carBinary.Fuel.Decrypt(LevelOfDecrypt);
                    int own_Weight = Convert.ToInt32(carBinary.Own_Weight.Decrypt(LevelOfDecrypt));
                    bool sold = Convert.ToBoolean(carBinary.Sold.Decrypt(LevelOfDecrypt));
                    if (sold)
                    {
                        soldcount++;
                    }
                    carsDecrypt.Add(new Cars
                    {
                        CarNumber = carNumber,
                        Model = model,
                        Color = color,
                        Number_of_seats = number_of_seats,
                        Cubic_capacity = cubic_capacity,
                        Mileage = mileage,
                        Year_of_production = year_of_production,
                        Chassis_number = chassis_number,
                        Engine_power = engine_power,
                        Gearbox = gearbox,
                        Fuel = fuel,
                        Own_Weight = own_Weight,
                        Sold = sold
                    });
                }
                stream.Close();
            }
            return carsDecrypt;
        }


        // WriteListToText + DateTime.Now
        static void WriteToText(List<Cars> cars, int LevelOfEncrypt)
        {
            string date = DateTime.Now.ToString().Replace(':', '-');

            StreamWriter writer = new StreamWriter(date + " " + FileName + LevelOfEncrypt + " level.csv");

            foreach (Cars car in cars)
            {
                string line1 =
                    $"{Convert.ToString(car.CarNumber).Encrypt(LevelOfEncrypt)};" +
                    $"{car.Model.Encrypt(LevelOfEncrypt)};" +
                    $"{car.Color.Encrypt(LevelOfEncrypt)};" +
                    $"{Convert.ToString(car.Number_of_seats).Encrypt(LevelOfEncrypt)};" +
                    $"{Convert.ToString(car.Cubic_capacity).Encrypt(LevelOfEncrypt)};" +
                    $"{Convert.ToString(car.Mileage).Encrypt(LevelOfEncrypt)};" +
                    $"{Convert.ToString(car.Year_of_production).Encrypt(LevelOfEncrypt)};" +
                    $"{car.Chassis_number.Encrypt(LevelOfEncrypt)};" +
                    $"{Convert.ToString(car.Engine_power).Encrypt(LevelOfEncrypt)};" +
                    $"{car.Gearbox.Encrypt(LevelOfEncrypt)};" +
                    $"{car.Fuel.Encrypt(LevelOfEncrypt)};" +
                    $"{Convert.ToString(car.Own_Weight).Encrypt(LevelOfEncrypt)};" +
                    $"{Convert.ToString(car.Sold).Encrypt(LevelOfEncrypt)}";
                writer.WriteLine(line1);
            }
            writer.Close();
        }

        //  WriteListToBin + DateTime.Now
        static void WriteToBin(List<Cars> cars, int LevelOfEncrypt)
        {
            string date = DateTime.Now.ToString().Replace(':', '-');

            FileStream stream = new FileStream(date + " " + FileName + LevelOfEncrypt + " level.dat", FileMode.Append, FileAccess.Write);
            BinaryFormatter formatter = new BinaryFormatter();

            foreach (Cars car in cars)
            {
                CarsBinary carsBinary = new CarsBinary()
                {
                    CarNumber = Convert.ToString(car.CarNumber).Encrypt(LevelOfEncrypt),
                    Model = Convert.ToString(car.Model).Encrypt(LevelOfEncrypt),
                    Color = Convert.ToString(car.Color).Encrypt(LevelOfEncrypt),
                    Number_of_seats = Convert.ToString(car.Number_of_seats).Encrypt(LevelOfEncrypt),
                    Cubic_capacity = Convert.ToString(car.Cubic_capacity).Encrypt(LevelOfEncrypt),
                    Mileage = Convert.ToString(car.Mileage).Encrypt(LevelOfEncrypt),
                    Year_of_production = Convert.ToString(car.Year_of_production).Encrypt(LevelOfEncrypt),
                    Chassis_number = Convert.ToString(car.Chassis_number).Encrypt(LevelOfEncrypt),
                    Engine_power = Convert.ToString(car.Engine_power).Encrypt(LevelOfEncrypt),
                    Gearbox = Convert.ToString(car.Gearbox).Encrypt(LevelOfEncrypt),
                    Fuel = Convert.ToString(car.Fuel).Encrypt(LevelOfEncrypt),
                    Own_Weight = Convert.ToString(car.Own_Weight).Encrypt(LevelOfEncrypt),
                    Sold = Convert.ToString(car.Sold).Encrypt(LevelOfEncrypt)
                };
                formatter.Serialize(stream, carsBinary);
            }
            stream.Close();
        }


        // Conversion to Codec Class
        public static void EncryptionTextToText(int LevelOfSource, int LevelOfTarget)
        {
            List<Cars> cars = ReadFromText(LevelOfSource);
            if (cars.Count > 0)
            {
                WriteToText(cars, LevelOfTarget);
            }
            else
            {
                MessageBox.Show("Bitte kopieren Sie " + FileName + LevelOfSource + " level.csv in den Ordner");
            }
        }

        public static void EncryptionTextToBin(int LevelOfSource, int LevelOfTarget)
        {
            List<Cars> cars = ReadFromText(LevelOfSource);

            if (cars.Count > 0)
            {
                WriteToBin(cars, LevelOfTarget);
            }
            else
            {
                MessageBox.Show("Bitte kopieren Sie " + FileName + LevelOfSource + " level.csv in den Ordner");
            }
        }

        public static void EncryptionBinToBin(int LevelOfSource, int LevelOfTarget)
        {
            List<Cars> cars = ReadFromBin(LevelOfSource);
            if (cars.Count > 0)
            {
                WriteToBin(cars, LevelOfTarget);
            }
            else
            {
                MessageBox.Show("Bitte kopieren Sie " + FileName + LevelOfSource + " level.csv in den Ordner");
            }
        }

        public static void EncryptionBinToText(int LevelOfSource, int LevelOfTarget)
        {
            List<Cars> cars = ReadFromBin(LevelOfSource);

            if (cars.Count > 0)
            {
                WriteToText(cars, LevelOfTarget);
            }
            else
            {
                MessageBox.Show("Bitte kopieren Sie " + FileName + LevelOfTarget + " level.dat in den Ordner");
            }
        }

        [Serializable]
        public class CarsBinary
        {
            public string CarNumber { get; set; }

            public string Model { get; set; }

            public string Color { get; set; }

            public string Number_of_seats { get; set; }

            public string Cubic_capacity { get; set; }

            public string Mileage { get; set; }

            public string Year_of_production { get; set; }

            public string Chassis_number { get; set; }

            public string Engine_power { get; set; }

            public string Gearbox { get; set; }

            public string Fuel { get; set; }

            public string Own_Weight { get; set; }

            public string Sold { get; set; }


        }

        [Serializable]
        public class Car
        {
            private static int _carNumber;

            private string _number;

            private string _model;

            private string _color;

            private int _number_of_seats;

            private double _cubic_capacity;

            private int _mileage;

            private int _year_of_production;

            private string _chassis_number;

            private int _engine_power;

            private string _gearbox;

            private string _fuel;

            private int _own_Weight;

            private bool _sold;

            public Car(string model, string color, int number_of_seats,
                double cubic_capacity, int mileage, int year_of_production, string chassis_number,
                int engine_power, string gearbox, string fuel, int own_Weight)
            {
                string storedId = null;
                if (File.Exists("aicar")) storedId = File.ReadAllText("aicar");
                if (storedId == null || storedId.Length < 0) storedId = "0";
                _carNumber = Convert.ToInt32(storedId) + 1;
                File.WriteAllText("aicar", _carNumber.ToString());
                this._number = _carNumber.ToString();
                Model = model;
                Color = color;
                Number_of_seats = number_of_seats;
                Cubic_capacity = cubic_capacity;
                Mileage = mileage;
                Year_of_production = year_of_production;
                Chassis_number = chassis_number;
                Engine_power = engine_power;
                Gearbox = gearbox;
                Fuel = fuel;
                Own_Weight = own_Weight;
                Sold = false;
            }

            public Car SetAsSold() { Sold = false; return this; }

            public static int CarNumber { get => _carNumber; set => _carNumber = value; }

            public string Number { get => _number; set => _number = value; }

            public string Model { get => _model; set => _model = value; }

            public string Color { get => _color; set => _color = value; }

            public int Number_of_seats { get => _number_of_seats; set => _number_of_seats = value; }

            public double Cubic_capacity { get => _cubic_capacity; set => _cubic_capacity = value; }

            public int Mileage { get => _mileage; set => _mileage = value; }

            public int Year_of_production { get => _year_of_production; set => _year_of_production = value; }

            public string Chassis_number { get => _chassis_number; set => _chassis_number = value; }

            public int Engine_power { get => _engine_power; set => _engine_power = value; }

            public string Gearbox { get => _gearbox; set => _gearbox = value; }

            public string Fuel { get => _fuel; set => _fuel = value; }

            public int Own_Weight { get => _own_Weight; set => _own_Weight = value; }

            public bool Sold { get => _sold; set => _sold = value; }
        }
    }
}
