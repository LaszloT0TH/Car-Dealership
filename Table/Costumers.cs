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
    public class Costumers
    {
        public static string FileName = "Costumer ";

        public int CostumerId { get; set; }

        public string Sex { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string Street { get; set; }

        public int PostalCcode { get; set; }

        public string Location { get; set; }

        public string Country { get; set; }

        public decimal TelNr { get; set; }

        public string Email { get; set; }

        public DateTime Date { get; set; }

        // Writing user input Creating a new record
        public static void WriteAddTextOrBin(Costumer costumer, int LevelOfAppEncryption, bool TextTrue_BinFalse)
        {
            if (TextTrue_BinFalse)
            {
                StreamWriter writer = new StreamWriter(FileName + LevelOfAppEncryption + " level.csv", true);
                string line =
                $"{Convert.ToString(costumer.Number).Encrypt(LevelOfAppEncryption)};" +
                $"{costumer.Sex.Encrypt(LevelOfAppEncryption)};" +
                $"{costumer.LastName.Encrypt(LevelOfAppEncryption)};" +
                $"{costumer.FirstName.Encrypt(LevelOfAppEncryption)};" +
                $"{costumer.Street.Encrypt(LevelOfAppEncryption)};" +
                $"{Convert.ToString(costumer.PostalCcode).Encrypt(LevelOfAppEncryption)};" +
                $"{costumer.Location.Encrypt(LevelOfAppEncryption)};" +
                $"{costumer.Country.Encrypt(LevelOfAppEncryption)};" +
                $"{Convert.ToString(costumer.TelNr).Encrypt(LevelOfAppEncryption)};" +
                $"{costumer.Email.Encrypt(LevelOfAppEncryption)}";
                writer.WriteLine(line);
                writer.Close();
            }

            else
            {
                FileStream stream = new FileStream(FileName + LevelOfAppEncryption + " level.dat", FileMode.Append, FileAccess.Write);

                BinaryFormatter formatter = new BinaryFormatter();

                CostumersBinary costumerBinary = new CostumersBinary()
                {
                    CostumerId = Convert.ToString(costumer.Number).Encrypt(LevelOfAppEncryption),
                    Sex = Convert.ToString(costumer.Sex).Encrypt(LevelOfAppEncryption),
                    LastName = Convert.ToString(costumer.LastName).Encrypt(LevelOfAppEncryption),
                    FirstName = Convert.ToString(costumer.FirstName).Encrypt(LevelOfAppEncryption),
                    Street = Convert.ToString(costumer.Street).Encrypt(LevelOfAppEncryption),
                    PostalCcode = Convert.ToString(costumer.PostalCcode).Encrypt(LevelOfAppEncryption),
                    Location = Convert.ToString(costumer.Location).Encrypt(LevelOfAppEncryption),
                    Country = Convert.ToString(costumer.Country).Encrypt(LevelOfAppEncryption),
                    TelNr = Convert.ToString(costumer.TelNr).Encrypt(LevelOfAppEncryption),
                    Email = Convert.ToString(costumer.Email).Encrypt(LevelOfAppEncryption)
                };

                formatter.Serialize(stream, costumerBinary);
                stream.Close();
            }
        }

        // Reading List 
        public static List<Costumers> ReadListTextOrBin(int LevelOfAppEncryption, bool TextTrue_BinFalse)
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
        static void WriteListToText(List<Costumers> costumers, int LevelOfEncrypt)
        {
            StreamWriter writer = new StreamWriter(FileName + LevelOfEncrypt + " level.csv");

            foreach (Costumers costumer in costumers)
            {
                string lines = $"{Convert.ToString(costumer.CostumerId).Encrypt(LevelOfEncrypt)};" +
                    $"{costumer.Sex.Encrypt(LevelOfEncrypt)};" +
                    $"{costumer.LastName.Encrypt(LevelOfEncrypt)};" +
                    $"{costumer.FirstName.Encrypt(LevelOfEncrypt)};" +
                    $"{costumer.Street.Encrypt(LevelOfEncrypt)};" +
                    $"{Convert.ToString(costumer.PostalCcode).Encrypt(LevelOfEncrypt)};" +
                    $"{costumer.Location.Encrypt(LevelOfEncrypt)};" +
                    $"{costumer.Country.Encrypt(LevelOfEncrypt)};" +
                    $"{Convert.ToString(costumer.TelNr).Encrypt(LevelOfEncrypt)};" +
                    $"{costumer.Email.Encrypt(LevelOfEncrypt)};" +
                    $"{Convert.ToString(costumer.Date).Encrypt(LevelOfEncrypt)}";
                writer.WriteLine(lines);
            }
            writer.Close();
        }

        // Writing from List to BinaryList
        static void WriteListToBin(List<Costumers> costumers, int LevelOfEncrypt)
        {
            FileStream stream = new FileStream(FileName + LevelOfEncrypt + " level.dat", FileMode.Append, FileAccess.Write);

            BinaryFormatter formatter = new BinaryFormatter();

            foreach (Costumers costumer in costumers)
            {
                CostumersBinary costumerBinary = new CostumersBinary()
                {
                    CostumerId = Convert.ToString(costumer.CostumerId).Encrypt(LevelOfEncrypt),
                    Sex = Convert.ToString(costumer.Sex).Encrypt(LevelOfEncrypt),
                    LastName = Convert.ToString(costumer.LastName).Encrypt(LevelOfEncrypt),
                    FirstName = Convert.ToString(costumer.FirstName).Encrypt(LevelOfEncrypt),
                    Street = Convert.ToString(costumer.Street).Encrypt(LevelOfEncrypt),
                    PostalCcode = Convert.ToString(costumer.PostalCcode).Encrypt(LevelOfEncrypt),
                    Location = Convert.ToString(costumer.Location).Encrypt(LevelOfEncrypt),
                    Country = Convert.ToString(costumer.Country).Encrypt(LevelOfEncrypt),
                    TelNr = Convert.ToString(costumer.TelNr).Encrypt(LevelOfEncrypt),
                    Email = Convert.ToString(costumer.Email).Encrypt(LevelOfEncrypt)
                };
                formatter.Serialize(stream, costumerBinary);
            }
            stream.Close();
        }

        public static void WriteListTextOrBin(List<Costumers> costumers, int LevelOfAppEncryption, bool TextTrue_BinFalse)
        {
            if (TextTrue_BinFalse)
            {
                WriteListToText(costumers, LevelOfAppEncryption);
            }

            else
            {
                WriteListToBin(costumers, LevelOfAppEncryption);
            }
        }


        static List<Costumers> ReadFromText(int LevelOfDecrypt)
        {
            List<Costumers> costumers = new List<Costumers>();

            if (File.Exists(FileName + LevelOfDecrypt + " level.csv"))
            {
                StreamReader reader = new StreamReader(FileName + LevelOfDecrypt + " level.csv", Encoding.Default);
                string line = reader.ReadLine();
                while (line != null)
                {
                    string[] record = line.Split(';');
                    int costumerId = Convert.ToInt32(record[0].Decrypt(LevelOfDecrypt));
                    string sex = record[1].Decrypt(LevelOfDecrypt);
                    string lastName = record[2].Decrypt(LevelOfDecrypt);
                    string firstName = record[3].Decrypt(LevelOfDecrypt);
                    string street = record[4].Decrypt(LevelOfDecrypt);
                    int postalCcode = Convert.ToInt32(record[5].Decrypt(LevelOfDecrypt));
                    string location = record[6].Decrypt(LevelOfDecrypt);
                    string country = record[7].Decrypt(LevelOfDecrypt);
                    decimal telNr = Convert.ToDecimal(record[8].Decrypt(LevelOfDecrypt));
                    string email = record[9].Decrypt(LevelOfDecrypt);
                    costumers.Add(new Costumers
                    {
                        CostumerId = costumerId,
                        Sex = sex,
                        LastName = lastName,
                        FirstName = firstName,
                        Street = street,
                        PostalCcode = postalCcode,
                        Location = location,
                        Country = country,
                        TelNr = telNr,
                        Email = email
                    });
                    line = reader.ReadLine();
                }
                reader.Close();
            }

            return costumers;
        }

        static List<Costumers> ReadFromBin(int LevelOfDecrypt)
        {
            List<Costumers> costumers = new List<Costumers>();
            if (File.Exists(FileName + LevelOfDecrypt + " level.dat"))
            {
                FileStream stream = new FileStream(FileName + LevelOfDecrypt + " level.dat", FileMode.Open, FileAccess.Read);
                BinaryFormatter formatter = new BinaryFormatter();
                while (stream.Position != stream.Length)
                {
                    CostumersBinary costumersBinary = (CostumersBinary)formatter.Deserialize(stream);

                    int costumerId = Convert.ToInt32(costumersBinary.CostumerId.Decrypt(LevelOfDecrypt));
                    string sex = costumersBinary.Sex.Decrypt(LevelOfDecrypt);
                    string lastName = costumersBinary.LastName.Decrypt(LevelOfDecrypt);
                    string firstName = costumersBinary.FirstName.Decrypt(LevelOfDecrypt);
                    string street = costumersBinary.Street.Decrypt(LevelOfDecrypt);
                    int postalCcode = Convert.ToInt32(costumersBinary.PostalCcode.Decrypt(LevelOfDecrypt));
                    string location = costumersBinary.Location.Decrypt(LevelOfDecrypt);
                    string country = costumersBinary.Country.Decrypt(LevelOfDecrypt);
                    decimal telNr = Convert.ToDecimal(costumersBinary.TelNr.Decrypt(LevelOfDecrypt));
                    string email = costumersBinary.Email.Decrypt(LevelOfDecrypt);
                    costumers.Add(new Costumers
                    {
                        CostumerId = costumerId,
                        Sex = sex,
                        LastName = lastName,
                        FirstName = firstName,
                        Street = street,
                        PostalCcode = postalCcode,
                        Location = location,
                        Country = country,
                        TelNr = telNr,
                        Email = email
                    });
                }
                stream.Close();
            }
            return costumers;
        }


        // WriteListToText + DateTime.Now
        static void WriteToText(List<Costumers> costumers, int LevelOfEncrypt)
        {
            string date = DateTime.Now.ToString().Replace(':', '-');

            StreamWriter writer = new StreamWriter(date + " " + FileName + LevelOfEncrypt + " level.csv");

            foreach (Costumers costumer in costumers)
            {
                string lines =
                    $"{Convert.ToString(costumer.CostumerId).Encrypt(LevelOfEncrypt)};" +
                    $"{costumer.Sex.Encrypt(LevelOfEncrypt)};" +
                    $"{costumer.LastName.Encrypt(LevelOfEncrypt)};" +
                    $"{costumer.FirstName.Encrypt(LevelOfEncrypt)};" +
                    $"{costumer.Street.Encrypt(LevelOfEncrypt)};" +
                    $"{Convert.ToString(costumer.PostalCcode).Encrypt(LevelOfEncrypt)};" +
                    $"{costumer.Location.Encrypt(LevelOfEncrypt)};" +
                    $"{costumer.Country.Encrypt(LevelOfEncrypt)};" +
                    $"{Convert.ToString(costumer.TelNr).Encrypt(LevelOfEncrypt)};" +
                    $"{costumer.Email.Encrypt(LevelOfEncrypt)};" +
                    $"{Convert.ToString(costumer.Date).Encrypt(LevelOfEncrypt)}";
                writer.WriteLine(lines);
            }
            writer.Close();
        }

        //  WriteListToBin + DateTime.Now
        static void WriteToBin(List<Costumers> costumers, int LevelOfEncrypt)
        {
            string date = DateTime.Now.ToString().Replace(':', '-');

            FileStream stream = new FileStream(date + " " + FileName + LevelOfEncrypt + " level.dat", FileMode.Append, FileAccess.Write);
            BinaryFormatter formatter = new BinaryFormatter();

            foreach (Costumers costumer in costumers)
            {
                CostumersBinary costumerBinary = new CostumersBinary()
                {
                    CostumerId = Convert.ToString(costumer.CostumerId).Encrypt(LevelOfEncrypt),
                    Sex = Convert.ToString(costumer.Sex).Encrypt(LevelOfEncrypt),
                    LastName = Convert.ToString(costumer.LastName).Encrypt(LevelOfEncrypt),
                    FirstName = Convert.ToString(costumer.FirstName).Encrypt(LevelOfEncrypt),
                    Street = Convert.ToString(costumer.Street).Encrypt(LevelOfEncrypt),
                    PostalCcode = Convert.ToString(costumer.PostalCcode).Encrypt(LevelOfEncrypt),
                    Location = Convert.ToString(costumer.Location).Encrypt(LevelOfEncrypt),
                    Country = Convert.ToString(costumer.Country).Encrypt(LevelOfEncrypt),
                    TelNr = Convert.ToString(costumer.TelNr).Encrypt(LevelOfEncrypt),
                    Email = Convert.ToString(costumer.Email).Encrypt(LevelOfEncrypt)
                };
                formatter.Serialize(stream, costumerBinary);
            }
            stream.Close();
        }


        // Conversion to Codec Class
        public static void EncryptionTextToText(int LevelOfSource, int LevelOfTarget)
        {
            List<Costumers> costumers = ReadFromText(LevelOfSource);
            if (costumers.Count > 0)
            {
                WriteToText(costumers, LevelOfTarget);
            }
            else
            {
                MessageBox.Show("Bitte kopieren Sie " + FileName + LevelOfSource + " level.csv in den Ordner");
            }
        }

        public static void EncryptionTextToBin(int LevelOfSource, int LevelOfTarget)
        {
            List<Costumers> costumers = ReadFromText(LevelOfSource);

            if (costumers.Count > 0)
            {
                WriteToBin(costumers, LevelOfTarget);
            }
            else
            {
                MessageBox.Show("Bitte kopieren Sie " + FileName + LevelOfSource + " level.csv in den Ordner");
            }
        }

        public static void EncryptionBinToBin(int LevelOfSource, int LevelOfTarget)
        {
            List<Costumers> costumers = ReadFromBin(LevelOfSource);
            if (costumers.Count > 0)
            {
                WriteToBin(costumers, LevelOfTarget);
            }
            else
            {
                MessageBox.Show("Bitte kopieren Sie " + FileName + LevelOfSource + " level.csv in den Ordner");
            }
        }

        public static void EncryptionBinToText(int LevelOfSource, int LevelOfTarget)
        {
            List<Costumers> costumers = ReadFromBin(LevelOfSource);

            if (costumers.Count > 0)
            {
                WriteToText(costumers, LevelOfTarget);
            }
            else
            {
                MessageBox.Show("Bitte kopieren Sie " + FileName + LevelOfTarget + " level.dat in den Ordner");
            }
        }

        [Serializable]
        class CostumersBinary
        {
            public string CostumerId { get; set; }

            public string Sex { get; set; }

            public string LastName { get; set; }

            public string FirstName { get; set; }

            public string Street { get; set; }

            public string PostalCcode { get; set; }

            public string Location { get; set; }

            public string Country { get; set; }

            public string TelNr { get; set; }

            public string Email { get; set; }

            public string Date { get; set; }
        }

        [Serializable]
        public class Costumer
        {
            private static int _costumerId = 0;

            private string _number;

            private string _sex;

            private string _lastName;

            private string _firstName;

            private string _street;

            private int _postalCcode;

            private string _location;

            private string _country;

            private decimal _telNr;

            private string _email;

            public Costumer(string sex, string lastName, string firstName, string street,
                int postalCcode, string location, string country, decimal telNr, string email)
            {
                string storedId = null;
                if (File.Exists("aicostumer")) storedId = File.ReadAllText("aicostumer");
                if (storedId == null || storedId.Length < 0) storedId = "0";
                _costumerId = Convert.ToInt32(storedId) + 1;
                File.WriteAllText("aicostumer", _costumerId.ToString());
                this._number = _costumerId.ToString();
                Sex = sex;
                LastName = lastName;
                FirstName = firstName;
                Street = street;
                PostalCcode = postalCcode;
                Location = location;
                Country = country;
                TelNr = telNr;
                Email = email;
            }

            public static int CostumerId { get => _costumerId; set => _costumerId = value; }

            public string Number { get => _number; set => _number = value; }

            public string Sex { get => _sex; set => _sex = value; }

            public string LastName { get => _lastName; set => _lastName = value; }

            public string FirstName { get => _firstName; set => _firstName = value; }

            public string Street { get => _street; set => _street = value; }

            public int PostalCcode { get => _postalCcode; set => _postalCcode = value; }

            public string Location { get => _location; set => _location = value; }

            public string Country { get => _country; set => _country = value; }

            public decimal TelNr { get => _telNr; set => _telNr = value; }

            public string Email { get => _email; set => _email = value; }
        }
    }
}