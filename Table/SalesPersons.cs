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
    public class SalesPersons
    {
        public static string FileName = "Salesperson ";

        public int _salesId { get; set; }

        public string _lastName { get; set; }

        public string _firstName { get; set; }

        public double _provision { get; set; }

        public DateTime _entry { get; set; }

        public bool _active { get; set; }

        public static int ActivePersonCounter = 0;


        // Writing user input Creating a new record
        public static void WriteAddTextOrBin(SalesPerson salesPerson, int LevelOfAppEncryption, bool TextTrue_BinFalse)
        {
            if (TextTrue_BinFalse)
            {
                StreamWriter writer = new StreamWriter(FileName + LevelOfAppEncryption + " level.csv", true);
                string line =
                $"{Convert.ToString(salesPerson.Number).Encrypt(LevelOfAppEncryption)};" +
                $"{salesPerson.LastName.Encrypt(LevelOfAppEncryption)};" +
                $"{salesPerson.FirstName.Encrypt(LevelOfAppEncryption)};" +
                $"{Convert.ToString(salesPerson.Provision).Encrypt(LevelOfAppEncryption)};" +
                $"{Convert.ToString(salesPerson.Entry).Encrypt(LevelOfAppEncryption)};" +
                $"{Convert.ToString(salesPerson.Active).Encrypt(LevelOfAppEncryption)}";
                writer.WriteLine(line);
                writer.Close();
            }

            else
            {
                FileStream stream = new FileStream(FileName + LevelOfAppEncryption + " level.dat", FileMode.Append, FileAccess.Write);

                BinaryFormatter formatter = new BinaryFormatter();

                SalesPersonsBinary salesPersonsBinary = new SalesPersonsBinary()
                {
                    salesId = Convert.ToString(salesPerson.Number).Encrypt(LevelOfAppEncryption),
                    lastName = Convert.ToString(salesPerson.LastName).Encrypt(LevelOfAppEncryption),
                    firstName = Convert.ToString(salesPerson.FirstName).Encrypt(LevelOfAppEncryption),
                    provision = Convert.ToString(salesPerson.Provision).Encrypt(LevelOfAppEncryption),
                    entry = Convert.ToString(salesPerson.Entry).Encrypt(LevelOfAppEncryption),
                    active = Convert.ToString(salesPerson.Active).Encrypt(LevelOfAppEncryption)
                };

                formatter.Serialize(stream, salesPersonsBinary);
                stream.Close();
            }
        }

        // Reading List 
        public static List<SalesPersons> ReadListTextOrBin(int LevelOfAppEncryption, bool TextTrue_BinFalse)
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
        static void WriteListToText(List<SalesPersons> salesPersons, int LevelOfEncrypt, object int32temp_salesId)
        {
            StreamWriter writer = new StreamWriter(FileName + LevelOfEncrypt + " level.csv");

            foreach (SalesPersons salesPerson in salesPersons)
            {
                if (int32temp_salesId != null)
                {
                    if (salesPerson._salesId == (int)int32temp_salesId)
                    {
                        salesPerson._active = false;
                    }
                }
                string lines =
                    $"{Convert.ToString(salesPerson._salesId).Encrypt(LevelOfEncrypt)};" +
                    $"{salesPerson._lastName.Encrypt(LevelOfEncrypt)};" +
                    $"{salesPerson._firstName.Encrypt(LevelOfEncrypt)};" +
                    $"{Convert.ToString(salesPerson._provision).Encrypt(LevelOfEncrypt)};" +
                    $"{Convert.ToString(salesPerson._entry).Encrypt(LevelOfEncrypt)};" +
                    $"{Convert.ToString(salesPerson._active).Encrypt(LevelOfEncrypt)}";
                writer.WriteLine(lines);
            }
            writer.Close();
        }

        // Writing from List to BinaryList
        static void WriteListToBin(List<SalesPersons> salesPersons, int LevelOfEncrypt, object int32temp_salesId)
        {
            FileStream stream = new FileStream(FileName + LevelOfEncrypt + " level.dat", FileMode.Append, FileAccess.Write);

            BinaryFormatter formatter = new BinaryFormatter();

            foreach (SalesPersons salesPerson in salesPersons)
            {
                if (int32temp_salesId != null)
                {
                    if (salesPerson._salesId == (int)int32temp_salesId)
                    {
                        salesPerson._active = false;
                    }
                }
                SalesPersonsBinary salesPersonsBinary = new SalesPersonsBinary()
                {
                    salesId = Convert.ToString(salesPerson._salesId).Encrypt(LevelOfEncrypt),
                    lastName = Convert.ToString(salesPerson._lastName).Encrypt(LevelOfEncrypt),
                    firstName = Convert.ToString(salesPerson._firstName).Encrypt(LevelOfEncrypt),
                    provision = Convert.ToString(salesPerson._provision).Encrypt(LevelOfEncrypt),
                    entry = Convert.ToString(salesPerson._entry).Encrypt(LevelOfEncrypt),
                    active = Convert.ToString(salesPerson._active).Encrypt(LevelOfEncrypt)
                };
                formatter.Serialize(stream, salesPersonsBinary);
            }
            stream.Close();
        }

        public static void WriteListTextOrBin(List<SalesPersons> salesPersons, int LevelOfAppEncryption, bool TextTrue_BinFalse, object int32temp_salesId = null)
        {
            if (TextTrue_BinFalse)
            {
                WriteListToText(salesPersons, LevelOfAppEncryption, int32temp_salesId);
            }

            else
            {
                WriteListToBin(salesPersons, LevelOfAppEncryption, int32temp_salesId);
            }
        }


        static List<SalesPersons> ReadFromText(int LevelOfDecrypt)
        {
            List<SalesPersons> salesPersons = new List<SalesPersons>();

            if (File.Exists(FileName + LevelOfDecrypt + " level.csv"))
            {
                StreamReader reader = new StreamReader(FileName + LevelOfDecrypt + " level.csv", Encoding.Default);
                string line = reader.ReadLine();
                while (line != null)
                {
                    string[] record = line.Split(';');
                    int salesId = Convert.ToInt32(record[0].Decrypt(LevelOfDecrypt));
                    string lastName = record[1].Decrypt(LevelOfDecrypt);
                    string firstName = record[2].Decrypt(LevelOfDecrypt);
                    double provision = Convert.ToDouble(record[3].Decrypt(LevelOfDecrypt));
                    DateTime entry = Convert.ToDateTime(record[4].Decrypt(LevelOfDecrypt));
                    bool active = Convert.ToBoolean(record[5].Decrypt(LevelOfDecrypt));
                    if (!active)
                    {
                        ActivePersonCounter++;
                    }
                    salesPersons.Add(new SalesPersons
                    {
                        _salesId = salesId,
                        _lastName = lastName,
                        _firstName = firstName,
                        _provision = provision,
                        _entry = entry,
                        _active = active
                    });
                    line = reader.ReadLine();
                }
                reader.Close();
            }

            return salesPersons;
        }

        static List<SalesPersons> ReadFromBin(int LevelOfDecrypt)
        {
            List<SalesPersons> salesPersons = new List<SalesPersons>();
            if (File.Exists(FileName + LevelOfDecrypt + " level.dat"))
            {
                FileStream stream = new FileStream(FileName + LevelOfDecrypt + " level.dat", FileMode.Open, FileAccess.Read);
                BinaryFormatter formatter = new BinaryFormatter();
                while (stream.Position != stream.Length)
                {
                    SalesPersonsBinary salesPersonsBinary = (SalesPersonsBinary)formatter.Deserialize(stream);

                    int salesId = Convert.ToInt32(salesPersonsBinary.salesId.Decrypt(LevelOfDecrypt));
                    string lastName = salesPersonsBinary.lastName.Decrypt(LevelOfDecrypt);
                    string firstName = salesPersonsBinary.firstName.Decrypt(LevelOfDecrypt);
                    double provision = Convert.ToDouble(salesPersonsBinary.provision.Decrypt(LevelOfDecrypt));
                    DateTime entry = Convert.ToDateTime(salesPersonsBinary.entry.Decrypt(LevelOfDecrypt));
                    bool active = Convert.ToBoolean(salesPersonsBinary.active.Decrypt(LevelOfDecrypt));
                    if (!active)
                    {
                        ActivePersonCounter++;
                    }
                    salesPersons.Add(new SalesPersons
                    {
                        _salesId = salesId,
                        _lastName = lastName,
                        _firstName = firstName,
                        _provision = provision,
                        _entry = entry,
                        _active = active
                    });
                }
                stream.Close();
            }
            return salesPersons;
        }


        // WriteListToText + DateTime.Now
        static void WriteToText(List<SalesPersons> salesPersons, int LevelOfEncrypt)
        {
            string date = DateTime.Now.ToString().Replace(':', '-');

            StreamWriter writer = new StreamWriter(date + " " + FileName + LevelOfEncrypt + " level.csv");

            foreach (SalesPersons salesPerson in salesPersons)
            {
                string lines =
                    $"{Convert.ToString(salesPerson._salesId).Encrypt(LevelOfEncrypt)};" +
                    $"{salesPerson._lastName.Encrypt(LevelOfEncrypt)};" +
                    $"{salesPerson._firstName.Encrypt(LevelOfEncrypt)};" +
                    $"{Convert.ToString(salesPerson._provision).Encrypt(LevelOfEncrypt)};" +
                    $"{Convert.ToString(salesPerson._entry).Encrypt(LevelOfEncrypt)};" +
                    $"{Convert.ToString(salesPerson._active).Encrypt(LevelOfEncrypt)}";
                writer.WriteLine(lines);
            }
            writer.Close();
        }

        //  WriteListToBin + DateTime.Now
        static void WriteToBin(List<SalesPersons> salesPersons, int LevelOfEncrypt)
        {
            string date = DateTime.Now.ToString().Replace(':', '-');

            FileStream stream = new FileStream(date + " " + FileName + LevelOfEncrypt + " level.dat", FileMode.Append, FileAccess.Write);
            BinaryFormatter formatter = new BinaryFormatter();

            foreach (SalesPersons salesPerson in salesPersons)
            {
                SalesPersonsBinary salesPersonsBinary = new SalesPersonsBinary()
                {
                    salesId = Convert.ToString(salesPerson._salesId).Encrypt(LevelOfEncrypt),
                    lastName = Convert.ToString(salesPerson._lastName).Encrypt(LevelOfEncrypt),
                    firstName = Convert.ToString(salesPerson._firstName).Encrypt(LevelOfEncrypt),
                    provision = Convert.ToString(salesPerson._provision).Encrypt(LevelOfEncrypt),
                    entry = Convert.ToString(salesPerson._entry).Encrypt(LevelOfEncrypt),
                    active = Convert.ToString(salesPerson._active).Encrypt(LevelOfEncrypt)
                };
                formatter.Serialize(stream, salesPersonsBinary);
            }
            stream.Close();
        }


        // Conversion to Codec Class
        public static void EncryptionTextToText(int LevelOfSource, int LevelOfTarget)
        {
            List<SalesPersons> salesPersons = ReadFromText(LevelOfSource);
            if (salesPersons.Count > 0)
            {
                WriteToText(salesPersons, LevelOfTarget);
            }
            else
            {
                MessageBox.Show("Bitte kopieren Sie " + FileName + LevelOfSource + " level.csv in den Ordner");
            }
        }

        public static void EncryptionTextToBin(int LevelOfSource, int LevelOfTarget)
        {
            List<SalesPersons> salesPersons = ReadFromText(LevelOfSource);

            if (salesPersons.Count > 0)
            {
                WriteToBin(salesPersons, LevelOfTarget);
            }
            else
            {
                MessageBox.Show("Bitte kopieren Sie " + FileName + LevelOfSource + " level.csv in den Ordner");
            }
        }

        public static void EncryptionBinToBin(int LevelOfSource, int LevelOfTarget)
        {
            List<SalesPersons> salesPersons = ReadFromBin(LevelOfSource);
            if (salesPersons.Count > 0)
            {
                WriteToBin(salesPersons, LevelOfTarget);
            }
            else
            {
                MessageBox.Show("Bitte kopieren Sie " + FileName + LevelOfSource + " level.csv in den Ordner");
            }
        }

        public static void EncryptionBinToText(int LevelOfSource, int LevelOfTarget)
        {
            List<SalesPersons> salesPersons = ReadFromBin(LevelOfSource);

            if (salesPersons.Count > 0)
            {
                WriteToText(salesPersons, LevelOfTarget);
            }
            else
            {
                MessageBox.Show("Bitte kopieren Sie " + FileName + LevelOfTarget + " level.dat in den Ordner");
            }
        }

        [Serializable]
        class SalesPersonsBinary
        {
            public string salesId { get; set; }

            public string lastName { get; set; }

            public string firstName { get; set; }

            public string provision { get; set; }

            public string entry { get; set; }

            public string active { get; set; }
        }

        [Serializable]
        public class SalesPerson
        {
            private static int _salesId = 0;

            private string _number;

            private string _lastName;

            private string _firstName;

            private double _provision;

            private DateTime _entry;

            private bool _active;

            public SalesPerson(string lastName, string firstName, double provision, DateTime entry, bool active = true)
            {
                string storedId = null;
                if (File.Exists("aisalesperson")) storedId = File.ReadAllText("aisalesperson");
                if (storedId == null || storedId.Length < 0) storedId = "0";
                _salesId = Convert.ToInt32(storedId) + 1;
                File.WriteAllText("aisalesperson", _salesId.ToString());
                this._number = _salesId.ToString();
                LastName = lastName;
                FirstName = firstName;
                Provision = provision;
                Entry = entry;
                Active = active;
            }

            public static int SalesId { get => _salesId; set => _salesId = value; }

            public string Number { get => _number; set => _number = value; }

            public string LastName { get => _lastName; set => _lastName = value; }

            public string FirstName { get => _firstName; set => _firstName = value; }

            public double Provision { get => _provision; set => _provision = value; }

            public DateTime Entry { get => _entry; set => _entry = value; }

            public bool Active { get => _active; set => _active = value; }
        }
    }
}
