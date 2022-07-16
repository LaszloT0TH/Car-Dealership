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
    public class SalesPersons_Secret_Data
    {
        public static string FileName = "Salesperson_Secret_Data ";

        public int _salesId { get; set; }

        public double _wage { get; set; }

        public DateTime _date_of_birth { get; set; }

        public int _postalCcode { get; set; }

        public string _location { get; set; }

        public string _street { get; set; }


        // Writing user input Creating a new record
        public static void WriteAddTextOrBin(Salesperson_Secret_Data salesperson_Secret_Data, int LevelOfAppEncryption, bool TextTrue_BinFalse)
        {
            if (TextTrue_BinFalse)
            {
                StreamWriter writer = new StreamWriter(FileName + LevelOfAppEncryption + " level.csv", true);
                string line =
                $"{Convert.ToString(salesperson_Secret_Data.Number).Encrypt(LevelOfAppEncryption)};" +
                $"{Convert.ToString(salesperson_Secret_Data.Wage).Encrypt(LevelOfAppEncryption)};" +
                $"{Convert.ToString(salesperson_Secret_Data.Date_of_birth).Encrypt(LevelOfAppEncryption)};" +
                $"{Convert.ToString(salesperson_Secret_Data.PostalCode).Encrypt(LevelOfAppEncryption)};" +
                $"{Convert.ToString(salesperson_Secret_Data.Location).Encrypt(LevelOfAppEncryption)};" +
                $"{Convert.ToString(salesperson_Secret_Data.Street).Encrypt(LevelOfAppEncryption)}";
                writer.WriteLine(line);
                writer.Close();
            }

            else
            {
                FileStream stream = new FileStream(FileName + LevelOfAppEncryption + " level.dat", FileMode.Append, FileAccess.Write);

                BinaryFormatter formatter = new BinaryFormatter();

                SalesPersons_Secret_DataBinary salesPersonsBinary = new SalesPersons_Secret_DataBinary()
                {
                    salesId = Convert.ToString(salesperson_Secret_Data.Number).Encrypt(LevelOfAppEncryption),
                    wage = Convert.ToString(salesperson_Secret_Data.Wage).Encrypt(LevelOfAppEncryption),
                    date_of_birth = Convert.ToString(salesperson_Secret_Data.Date_of_birth).Encrypt(LevelOfAppEncryption),
                    postalCcode = Convert.ToString(salesperson_Secret_Data.PostalCode).Encrypt(LevelOfAppEncryption),
                    location = Convert.ToString(salesperson_Secret_Data.Location).Encrypt(LevelOfAppEncryption),
                    street = Convert.ToString(salesperson_Secret_Data.Street).Encrypt(LevelOfAppEncryption)
                };

                formatter.Serialize(stream, salesPersonsBinary);
                stream.Close();
            }
        }

        // Reading List 
        public static List<SalesPersons_Secret_Data> ReadListTextOrBin(int LevelOfAppEncryption, bool TextTrue_BinFalse)
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
        static void WriteListToText(List<SalesPersons_Secret_Data> salespersons_Secret_Datas, int LevelOfEncrypt)
        {
            StreamWriter writer = new StreamWriter(FileName + LevelOfEncrypt + " level.csv");

            foreach (SalesPersons_Secret_Data salespersons_Secret_Data in salespersons_Secret_Datas)
            {
                string lines =
                    $"{Convert.ToString(salespersons_Secret_Data._salesId).Encrypt(LevelOfEncrypt)};" +
                    $"{Convert.ToString(salespersons_Secret_Data._wage).Encrypt(LevelOfEncrypt)};" +
                    $"{Convert.ToString(salespersons_Secret_Data._date_of_birth).Encrypt(LevelOfEncrypt)};" +
                    $"{Convert.ToString(salespersons_Secret_Data._postalCcode).Encrypt(LevelOfEncrypt)};" +
                    $"{Convert.ToString(salespersons_Secret_Data._location).Encrypt(LevelOfEncrypt)};" +
                    $"{Convert.ToString(salespersons_Secret_Data._street).Encrypt(LevelOfEncrypt)}";
                writer.WriteLine(lines);
            }
            writer.Close();
        }

        // Writing from List to BinaryList
        static void WriteListToBin(List<SalesPersons_Secret_Data> salespersons_Secret_Datas, int LevelOfEncrypt)
        {
            FileStream stream = new FileStream(FileName + LevelOfEncrypt + " level.dat", FileMode.Append, FileAccess.Write);

            BinaryFormatter formatter = new BinaryFormatter();

            foreach (SalesPersons_Secret_Data salespersons_Secret_Data in salespersons_Secret_Datas)
            {
                SalesPersons_Secret_DataBinary salespersons_Secret_DataBinary = new SalesPersons_Secret_DataBinary()
                {
                    salesId = Convert.ToString(salespersons_Secret_Data._salesId).Encrypt(LevelOfEncrypt),
                    wage = Convert.ToString(salespersons_Secret_Data._wage).Encrypt(LevelOfEncrypt),
                    date_of_birth = Convert.ToString(salespersons_Secret_Data._date_of_birth).Encrypt(LevelOfEncrypt),
                    postalCcode = Convert.ToString(salespersons_Secret_Data._postalCcode).Encrypt(LevelOfEncrypt),
                    location = Convert.ToString(salespersons_Secret_Data._location).Encrypt(LevelOfEncrypt),
                    street = Convert.ToString(salespersons_Secret_Data._street).Encrypt(LevelOfEncrypt)
                };
                formatter.Serialize(stream, salespersons_Secret_DataBinary);
            }
            stream.Close();
        }

        public static void WriteListTextOrBin(List<SalesPersons_Secret_Data> salespersons_Secret_Datas, int LevelOfAppEncryption, bool TextTrue_BinFalse)
        {
            if (TextTrue_BinFalse)
            {
                WriteListToText(salespersons_Secret_Datas, LevelOfAppEncryption);
            }

            else
            {
                WriteListToBin(salespersons_Secret_Datas, LevelOfAppEncryption);
            }
        }


        static List<SalesPersons_Secret_Data> ReadFromText(int LevelOfDecrypt)
        {
            List<SalesPersons_Secret_Data> salespersons_Secret_Datas = new List<SalesPersons_Secret_Data>();

            if (File.Exists(FileName + LevelOfDecrypt + " level.csv"))
            {
                StreamReader reader = new StreamReader(FileName + LevelOfDecrypt + " level.csv", Encoding.Default);
                string line = reader.ReadLine();
                while (line != null)
                {
                    string[] record = line.Split(';');
                    int salesId = Convert.ToInt32(record[0].Decrypt(LevelOfDecrypt));
                    double wage = Convert.ToDouble(record[1].Decrypt(LevelOfDecrypt));
                    DateTime date_of_birth = Convert.ToDateTime(record[2].Decrypt(LevelOfDecrypt));
                    int postalCcode = Convert.ToInt32(record[3].Decrypt(LevelOfDecrypt));
                    string location = record[4].Decrypt(LevelOfDecrypt);
                    string street = record[5].Decrypt(LevelOfDecrypt);
                    salespersons_Secret_Datas.Add(new SalesPersons_Secret_Data
                    {
                        _salesId = salesId,
                        _wage = wage,
                        _date_of_birth = date_of_birth,
                        _postalCcode = postalCcode,
                        _location = location,
                        _street = street
                    });
                    line = reader.ReadLine();
                }
                reader.Close();
            }

            return salespersons_Secret_Datas;
        }

        static List<SalesPersons_Secret_Data> ReadFromBin(int LevelOfDecrypt)
        {
            List<SalesPersons_Secret_Data> salespersons_Secret_Datas = new List<SalesPersons_Secret_Data>();
            if (File.Exists(FileName + LevelOfDecrypt + " level.dat"))
            {
                FileStream stream = new FileStream(FileName + LevelOfDecrypt + " level.dat", FileMode.Open, FileAccess.Read);
                BinaryFormatter formatter = new BinaryFormatter();
                while (stream.Position != stream.Length)
                {
                    SalesPersons_Secret_DataBinary salesPersonsBinary = (SalesPersons_Secret_DataBinary)formatter.Deserialize(stream);

                    int salesId = Convert.ToInt32(salesPersonsBinary.salesId.Decrypt(LevelOfDecrypt));
                    double wage = Convert.ToDouble(salesPersonsBinary.wage.Decrypt(LevelOfDecrypt));
                    DateTime date_of_birth = Convert.ToDateTime(salesPersonsBinary.date_of_birth.Decrypt(LevelOfDecrypt));
                    int postalCcode = Convert.ToInt32(salesPersonsBinary.postalCcode.Decrypt(LevelOfDecrypt));
                    string location = Convert.ToString(salesPersonsBinary.location.Decrypt(LevelOfDecrypt));
                    string street = Convert.ToString(salesPersonsBinary.street.Decrypt(LevelOfDecrypt));
                    salespersons_Secret_Datas.Add(new SalesPersons_Secret_Data
                    {
                        _salesId = salesId,
                        _wage = wage,
                        _date_of_birth = date_of_birth,
                        _postalCcode = postalCcode,
                        _location = location,
                        _street = street
                    });
                }
                stream.Close();
            }
            return salespersons_Secret_Datas;
        }


        // WriteListToText + DateTime.Now
        static void WriteToText(List<SalesPersons_Secret_Data> salespersons_Secret_Datas, int LevelOfEncrypt)
        {
            string date = DateTime.Now.ToString().Replace(':', '-');

            StreamWriter writer = new StreamWriter(date + " " + FileName + LevelOfEncrypt + " level.csv");

            foreach (SalesPersons_Secret_Data salespersons_Secret_Data in salespersons_Secret_Datas)
            {
                string lines =
                    $"{Convert.ToString(salespersons_Secret_Data._salesId).Encrypt(LevelOfEncrypt)};" +
                    $"{Convert.ToString(salespersons_Secret_Data._wage).Encrypt(LevelOfEncrypt)};" +
                    $"{Convert.ToString(salespersons_Secret_Data._date_of_birth).Encrypt(LevelOfEncrypt)};" +
                    $"{Convert.ToString(salespersons_Secret_Data._postalCcode).Encrypt(LevelOfEncrypt)};" +
                    $"{Convert.ToString(salespersons_Secret_Data._location).Encrypt(LevelOfEncrypt)};" +
                    $"{Convert.ToString(salespersons_Secret_Data._street).Encrypt(LevelOfEncrypt)}";
                writer.WriteLine(lines);
            }
            writer.Close();
        }

        //  WriteListToBin + DateTime.Now
        static void WriteToBin(List<SalesPersons_Secret_Data> salespersons_Secret_Datas, int LevelOfEncrypt)
        {
            string date = DateTime.Now.ToString().Replace(':', '-');

            FileStream stream = new FileStream(date + " " + FileName + LevelOfEncrypt + " level.dat", FileMode.Append, FileAccess.Write);
            BinaryFormatter formatter = new BinaryFormatter();

            foreach (SalesPersons_Secret_Data salespersons_Secret_Data in salespersons_Secret_Datas)
            {
                SalesPersons_Secret_DataBinary salespersons_Secret_DataBinary = new SalesPersons_Secret_DataBinary()
                {
                    salesId = Convert.ToString(salespersons_Secret_Data._salesId).Encrypt(LevelOfEncrypt),
                    wage = Convert.ToString(salespersons_Secret_Data._wage).Encrypt(LevelOfEncrypt),
                    date_of_birth = Convert.ToString(salespersons_Secret_Data._date_of_birth).Encrypt(LevelOfEncrypt),
                    postalCcode = Convert.ToString(salespersons_Secret_Data._postalCcode).Encrypt(LevelOfEncrypt),
                    location = Convert.ToString(salespersons_Secret_Data._location).Encrypt(LevelOfEncrypt),
                    street = Convert.ToString(salespersons_Secret_Data._street).Encrypt(LevelOfEncrypt)
                };
                formatter.Serialize(stream, salespersons_Secret_DataBinary);
            }
            stream.Close();
        }


        // Conversion to Codec Class
        public static void EncryptionTextToText(int LevelOfSource, int LevelOfTarget)
        {
            List<SalesPersons_Secret_Data> salespersons_Secret_Data = ReadFromText(LevelOfSource);
            if (salespersons_Secret_Data.Count > 0)
            {
                WriteToText(salespersons_Secret_Data, LevelOfTarget);
            }
            else
            {
                MessageBox.Show("Bitte kopieren Sie " + FileName + LevelOfSource + " level.csv in den Ordner");
            }
        }

        public static void EncryptionTextToBin(int LevelOfSource, int LevelOfTarget)
        {
            List<SalesPersons_Secret_Data> salespersons_Secret_Data = ReadFromText(LevelOfSource);

            if (salespersons_Secret_Data.Count > 0)
            {
                WriteToBin(salespersons_Secret_Data, LevelOfTarget);
            }
            else
            {
                MessageBox.Show("Bitte kopieren Sie " + FileName + LevelOfSource + " level.csv in den Ordner");
            }
        }

        public static void EncryptionBinToBin(int LevelOfSource, int LevelOfTarget)
        {
            List<SalesPersons_Secret_Data> salespersons_Secret_Data = ReadFromBin(LevelOfSource);
            if (salespersons_Secret_Data.Count > 0)
            {
                WriteToBin(salespersons_Secret_Data, LevelOfTarget);
            }
            else
            {
                MessageBox.Show("Bitte kopieren Sie " + FileName + LevelOfSource + " level.csv in den Ordner");
            }
        }

        public static void EncryptionBinToText(int LevelOfSource, int LevelOfTarget)
        {
            List<SalesPersons_Secret_Data> salespersons_Secret_Data = ReadFromBin(LevelOfSource);

            if (salespersons_Secret_Data.Count > 0)
            {
                WriteToText(salespersons_Secret_Data, LevelOfTarget);
            }
            else
            {
                MessageBox.Show("Bitte kopieren Sie " + FileName + LevelOfTarget + " level.dat in den Ordner");
            }
        }

        [Serializable]
        class SalesPersons_Secret_DataBinary
        {
            public string salesId { get; set; }

            public string wage { get; set; }

            public string date_of_birth { get; set; }

            public string postalCcode { get; set; }

            public string location { get; set; }

            public string street { get; set; }
        }

        [Serializable]
        public class Salesperson_Secret_Data
        {
            private static int _sSalesId = 0;

            private string _number;

            private double _wage;

            private DateTime _date_of_birth;

            private int _postalCode;

            private string _location;

            private string _street;

            public Salesperson_Secret_Data(double wage, DateTime date_of_birth, int postalCode, string location, string street)
            {
                string storedId = null;
                if (File.Exists("aisalesperson")) storedId = File.ReadAllText("aisalesperson");
                if (storedId == null || storedId.Length < 0) storedId = "0";
                SSalesId = Convert.ToInt32(storedId);
                File.WriteAllText("aisalesperson", SSalesId.ToString());
                this._number = SSalesId.ToString();
                Wage = wage;
                Date_of_birth = date_of_birth;
                PostalCode = postalCode;
                Location = location;
                Street = street;
            }

            public static int SSalesId { get => _sSalesId; set => _sSalesId = value; }

            public string Number { get => _number; set => _number = value; }

            public double Wage { get => _wage; set => _wage = value; }

            public DateTime Date_of_birth { get => _date_of_birth; set => _date_of_birth = value; }

            public int PostalCode { get => _postalCode; set => _postalCode = value; }

            public string Location { get => _location; set => _location = value; }

            public string Street { get => _street; set => _street = value; }
        }
    }
}
