using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;

namespace Car_Dealership
{
    [Serializable]
    public class Sales
    {
        public static string FileName = "Sale ";

        public int _saleId { get; set; }

        public int _costumerId { get; set; }

        public int _salesId { get; set; }

        public int _carNumber { get; set; }

        public DateTime _date { get; set; }

        // Writing user input Creating a new record
        public static void WriteAddTextOrBin(Sale sale, int LevelOfAppEncryption, bool TextTrue_BinFalse)
        {
            if (TextTrue_BinFalse)
            {
                StreamWriter writer = new StreamWriter(FileName + LevelOfAppEncryption + " level.csv", true);
                string line =
                 $"{Convert.ToString(sale.Number).Encrypt(LevelOfAppEncryption)};" +
                 $"{Convert.ToString(sale.CostumerId).Encrypt(LevelOfAppEncryption)};" +
                 $"{Convert.ToString(sale.SalesId).Encrypt(LevelOfAppEncryption)};" +
                 $"{Convert.ToString(sale.CarNumber).Encrypt(LevelOfAppEncryption)};" +
                 $"{Convert.ToString(sale.Date).Encrypt(LevelOfAppEncryption)}";
                writer.WriteLine(line);
                writer.Close();
            }

            else
            {
                FileStream stream = new FileStream(FileName + LevelOfAppEncryption + " level.dat", FileMode.Append, FileAccess.Write);

                BinaryFormatter formatter = new BinaryFormatter();

                SalesBinary saleBinary = new SalesBinary()
                {
                    saleId = Convert.ToString(sale.Number).Encrypt(LevelOfAppEncryption),
                    costumerId = Convert.ToString(sale.CostumerId).Encrypt(LevelOfAppEncryption),
                    salesId = Convert.ToString(sale.SalesId).Encrypt(LevelOfAppEncryption),
                    carNumber = Convert.ToString(sale.CarNumber).Encrypt(LevelOfAppEncryption),
                    date = Convert.ToString(sale.Date).Encrypt(LevelOfAppEncryption)
                };

                formatter.Serialize(stream, saleBinary);
                stream.Close();
            }
        }

        // Reading List 
        public static List<Sales> ReadListTextOrBin(int LevelOfAppEncryption, bool TextTrue_BinFalse)
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
        static void WriteListToText(List<Sales> sales, int LevelOfEncrypt)
        {
            StreamWriter writer = new StreamWriter(FileName + LevelOfEncrypt + " level.csv");

            foreach (Sales sale in sales)
            {
                string line1 = $"{Convert.ToString(sale._saleId)};" +
                     $"{Convert.ToString(sale._costumerId)};" +
                     $"{Convert.ToString(sale._salesId)};" +
                     $"{Convert.ToString(sale._carNumber)};" +
                     $"{Convert.ToString(sale._date)}";
                writer.WriteLine(line1);
            }
            writer.Close();
        }

        // Writing from List to BinaryList
        static void WriteListToBin(List<Sales> sales, int LevelOfEncrypt)
        {
            FileStream stream = new FileStream(FileName + LevelOfEncrypt + " level.dat", FileMode.Append, FileAccess.Write);

            BinaryFormatter formatter = new BinaryFormatter();

            foreach (Sales sale in sales)
            {
                SalesBinary salesBinary = new SalesBinary()
                {
                    saleId = Convert.ToString(sale._saleId).Encrypt(LevelOfEncrypt),
                    costumerId = Convert.ToString(sale._costumerId).Encrypt(LevelOfEncrypt),
                    salesId = Convert.ToString(sale._salesId).Encrypt(LevelOfEncrypt),
                    carNumber = Convert.ToString(sale._carNumber).Encrypt(LevelOfEncrypt),
                    date = Convert.ToString(sale._date).Encrypt(LevelOfEncrypt)
                };
                formatter.Serialize(stream, salesBinary);
            }
            stream.Close();
        }

        public static void WriteListTextOrBin(List<Sales> sales, int LevelOfAppEncryption, bool TextTrue_BinFalse)
        {
            if (TextTrue_BinFalse)
            {
                WriteListToText(sales, LevelOfAppEncryption);
            }

            else
            {
                WriteListToBin(sales, LevelOfAppEncryption);
            }
        }


        static List<Sales> ReadFromText(int LevelOfDecrypt)
        {
            List<Sales> sales = new List<Sales>();

            if (File.Exists(FileName + LevelOfDecrypt + " level.csv"))
            {
                StreamReader reader = new StreamReader(FileName + LevelOfDecrypt + " level.csv", Encoding.Default);
                string line = reader.ReadLine();
                while (line != null)
                {
                    string[] record = line.Split(';');
                    int saleId = Convert.ToInt32(record[0].Decrypt(LevelOfDecrypt));
                    int costumerId = Convert.ToInt32(record[1].Decrypt(LevelOfDecrypt));
                    int salesId = Convert.ToInt32(record[2].Decrypt(LevelOfDecrypt));
                    int carNumber = Convert.ToInt32(record[3].Decrypt(LevelOfDecrypt));
                    DateTime dateTime = Convert.ToDateTime(record[4].Decrypt(LevelOfDecrypt));
                    sales.Add(new Sales
                    {
                        _saleId = saleId,
                        _costumerId = costumerId,
                        _salesId = salesId,
                        _carNumber = carNumber,
                        _date = dateTime
                    });
                    line = reader.ReadLine();
                }
                reader.Close();
            }

            return sales;
        }

        static List<Sales> ReadFromBin(int LevelOfDecrypt)
        {
            List<Sales> sales = new List<Sales>();
            if (File.Exists(FileName + LevelOfDecrypt + " level.dat"))
            {
                FileStream stream = new FileStream(FileName + LevelOfDecrypt + " level.dat", FileMode.Open, FileAccess.Read);
                BinaryFormatter formatter = new BinaryFormatter();
                while (stream.Position != stream.Length)
                {
                    SalesBinary saleBinary = (SalesBinary)formatter.Deserialize(stream);

                    int saleId = Convert.ToInt32(saleBinary.saleId.Decrypt(LevelOfDecrypt));
                    int costumerId = Convert.ToInt32(saleBinary.costumerId.Decrypt(LevelOfDecrypt));
                    int salesId = Convert.ToInt32(saleBinary.salesId.Decrypt(LevelOfDecrypt));
                    int carNumber = Convert.ToInt32(saleBinary.carNumber.Decrypt(LevelOfDecrypt));
                    DateTime dateTime = Convert.ToDateTime(saleBinary.date.Decrypt(LevelOfDecrypt));
                    sales.Add(new Sales
                    {
                        _saleId = saleId,
                        _costumerId = costumerId,
                        _salesId = salesId,
                        _carNumber = carNumber,
                        _date = dateTime
                    });
                }
                stream.Close();
            }
            return sales;
        }


        // WriteListToText + DateTime.Now
        static void WriteToText(List<Sales> sales, int LevelOfEncrypt)
        {
            string date = DateTime.Now.ToString().Replace(':', '-');

            StreamWriter writer = new StreamWriter(date + " " + FileName + LevelOfEncrypt + " level.csv");

            foreach (Sales sale in sales)
            {
                string line1 = $"{Convert.ToString(sale._saleId)};" +
                     $"{Convert.ToString(sale._costumerId)};" +
                     $"{Convert.ToString(sale._salesId)};" +
                     $"{Convert.ToString(sale._carNumber)};" +
                     $"{Convert.ToString(sale._date)}";
                writer.WriteLine(line1);
            }
            writer.Close();
        }

        //  WriteListToBin + DateTime.Now
        static void WriteToBin(List<Sales> sales, int LevelOfEncrypt)
        {
            string date = DateTime.Now.ToString().Replace(':', '-');

            FileStream stream = new FileStream(date + " " + FileName + LevelOfEncrypt + " level.dat", FileMode.Append, FileAccess.Write);
            BinaryFormatter formatter = new BinaryFormatter();

            foreach (Sales sale in sales)
            {
                SalesBinary salesBinary = new SalesBinary()
                {
                    saleId = Convert.ToString(sale._saleId).Encrypt(LevelOfEncrypt),
                    costumerId = Convert.ToString(sale._costumerId).Encrypt(LevelOfEncrypt),
                    salesId = Convert.ToString(sale._salesId).Encrypt(LevelOfEncrypt),
                    carNumber = Convert.ToString(sale._carNumber).Encrypt(LevelOfEncrypt),
                    date = Convert.ToString(sale._date).Encrypt(LevelOfEncrypt)
                };
                formatter.Serialize(stream, salesBinary);
            }
            stream.Close();
        }


        // Conversion to Codec Class
        public static void EncryptionTextToText(int LevelOfSource, int LevelOfTarget)
        {
            List<Sales> sales = ReadFromText(LevelOfSource);
            if (sales.Count > 0)
            {
                WriteToText(sales, LevelOfTarget);
            }
            else
            {
                MessageBox.Show("Bitte kopieren Sie " + FileName + LevelOfSource + " level.csv in den Ordner");
            }
        }

        public static void EncryptionTextToBin(int LevelOfSource, int LevelOfTarget)
        {
            List<Sales> sales = ReadFromText(LevelOfSource);

            if (sales.Count > 0)
            {
                WriteToBin(sales, LevelOfTarget);
            }
            else
            {
                MessageBox.Show("Bitte kopieren Sie " + FileName + LevelOfSource + " level.csv in den Ordner");
            }
        }

        public static void EncryptionBinToBin(int LevelOfSource, int LevelOfTarget)
        {
            List<Sales> sales = ReadFromBin(LevelOfSource);
            if (sales.Count > 0)
            {
                WriteToBin(sales, LevelOfTarget);
            }
            else
            {
                MessageBox.Show("Bitte kopieren Sie " + FileName + LevelOfSource + " level.csv in den Ordner");
            }
        }

        public static void EncryptionBinToText(int LevelOfSource, int LevelOfTarget)
        {
            List<Sales> sales = ReadFromBin(LevelOfSource);

            if (sales.Count > 0)
            {
                WriteToText(sales, LevelOfTarget);
            }
            else
            {
                MessageBox.Show("Bitte kopieren Sie " + FileName + LevelOfTarget + " level.dat in den Ordner");
            }
        }

        [Serializable]
        class SalesBinary
        {
            public string saleId { get; set; }

            public string costumerId { get; set; }

            public string salesId { get; set; }

            public string carNumber { get; set; }

            public string date { get; set; }
        }

        [Serializable]
        public class Sale
        {
            private static int _saleId;

            private string _number;

            private int _costumerId;

            private int _salesId;

            private int _carNumber;

            private DateTime _date;

            public Sale(int costumerId, int salesId, int carNumber, DateTime date)
            {
                string storedId = null;
                if (File.Exists("aisale")) storedId = File.ReadAllText("aisale");
                if (storedId == null || storedId.Length < 0) storedId = "0";
                _saleId = Convert.ToInt32(storedId) + 1;
                File.WriteAllText("aisale", _saleId.ToString());
                this._number = _saleId.ToString();
                CostumerId = costumerId;
                SalesId = salesId;
                CarNumber = carNumber;
                Date = date;
            }

            public static int SaleId { get => _saleId; set => _saleId = value; }

            public string Number { get => _number; set => _number = value; }

            public int CostumerId { get => _costumerId; set => _costumerId = value; }

            public int SalesId { get => _salesId; set => _salesId = value; }

            public int CarNumber { get => _carNumber; set => _carNumber = value; }

            public DateTime Date { get => _date; set => _date = value; }
        }
    }
}
