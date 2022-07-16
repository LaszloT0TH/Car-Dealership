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
    public class Logins
    {
        public static string FileName = "Login ";

        public string Username { get; set; }

        public string Password { get; set; }

        public DateTime Date { get; set; }


        // Writing user input Creating a new record
        public static void WriteAddTextOrBin(Logins log, int LevelOfAppEncryption, bool TextTrue_BinFalse)
        {
            if (TextTrue_BinFalse)
            {
                StreamWriter writer = new StreamWriter(FileName + LevelOfAppEncryption + " level.csv", true);
                string line =
                        $"{log.Username.Encrypt(LevelOfAppEncryption)};" +
                        $"{log.Password.Encrypt(LevelOfAppEncryption)};" +
                        $"{Convert.ToString(log.Date).Encrypt(LevelOfAppEncryption)}";
                writer.WriteLine(line);
                writer.Close();
            }

            else
            {
                FileStream stream = new FileStream(FileName + LevelOfAppEncryption + " level.dat", FileMode.Append, FileAccess.Write);

                BinaryFormatter formatter = new BinaryFormatter();

                LoginsBinary logBinary = new LoginsBinary()
                {
                    Username = log.Username.Encrypt(LevelOfAppEncryption),
                    Password = log.Password.Encrypt(LevelOfAppEncryption),
                    Date = Convert.ToString(log.Date).Encrypt(LevelOfAppEncryption)
                };
                formatter.Serialize(stream, logBinary);
                stream.Close();
            }
        }

        // Reading List 
        public static List<Logins> ReadListTextOrBin(int LevelOfAppEncryption, bool TextTrue_BinFalse)
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
        static void WriteListToText(List<Logins> logs, int LevelOfEncrypt)
        {
            StreamWriter writer = new StreamWriter(FileName + LevelOfEncrypt + " level.csv");

            foreach (Logins log in logs)
            {
                string lines =
                    $"{log.Username.Encrypt(LevelOfEncrypt)};" +
                    $"{log.Password.Encrypt(LevelOfEncrypt)};" +
                    $"{Convert.ToString(log.Date).Encrypt(LevelOfEncrypt)}";
                writer.WriteLine(lines);
            }
            writer.Close();
        }

        // Writing from List to BinaryList
        static void WriteListToBin(List<Logins> logs, int LevelOfEncrypt)
        {
            FileStream stream = new FileStream(FileName + LevelOfEncrypt + " level.dat", FileMode.Append, FileAccess.Write);

            BinaryFormatter formatter = new BinaryFormatter();

            foreach (Logins log in logs)
            {
                LoginsBinary costumerBinary = new LoginsBinary()
                {
                    Username = Convert.ToString(log.Username).Encrypt(LevelOfEncrypt),
                    Password = Convert.ToString(log.Password).Encrypt(LevelOfEncrypt),
                    Date = Convert.ToString(log.Date).Encrypt(LevelOfEncrypt)
                };
                formatter.Serialize(stream, costumerBinary);
            }
            stream.Close();
        }

        public static void WriteListTextOrBin(List<Logins> logs, int LevelOfAppEncryption, bool TextTrue_BinFalse)
        {
            if (TextTrue_BinFalse)
            {
                WriteListToText(logs, LevelOfAppEncryption);
            }

            else
            {
                WriteListToBin(logs, LevelOfAppEncryption);
            }
        }


        static List<Logins> ReadFromText(int LevelOfDecrypt)
        {
            List<Logins> logs = new List<Logins>();

            if (File.Exists(FileName + LevelOfDecrypt + " level.csv"))
            {
                StreamReader reader = new StreamReader(FileName + LevelOfDecrypt + " level.csv", Encoding.Default);
                string line = reader.ReadLine();
                while (line != null)
                {
                    string[] record = line.Split(';');
                    string username = record[0].Decrypt(LevelOfDecrypt);
                    string password = record[1].Decrypt(LevelOfDecrypt);
                    DateTime date = Convert.ToDateTime(record[2].Decrypt(LevelOfDecrypt));
                    logs.Add(new Logins
                    {
                        Username = username,
                        Password = password,
                        Date = date
                    });
                    line = reader.ReadLine();
                }
                reader.Close();
            }

            return logs;
        }

        static List<Logins> ReadFromBin(int LevelOfDecrypt)
        {
            List<Logins> logs = new List<Logins>();
            if (File.Exists(FileName + LevelOfDecrypt + " level.dat"))
            {
                FileStream stream = new FileStream(FileName + LevelOfDecrypt + " level.dat", FileMode.Open, FileAccess.Read);
                BinaryFormatter formatter = new BinaryFormatter();
                while (stream.Position != stream.Length)
                {
                    LoginsBinary logsBinary = (LoginsBinary)formatter.Deserialize(stream);

                    string username = logsBinary.Username.Decrypt(LevelOfDecrypt);
                    string password = logsBinary.Password.Decrypt(LevelOfDecrypt);
                    DateTime date = Convert.ToDateTime(logsBinary.Date.Decrypt(LevelOfDecrypt));
                    logs.Add(new Logins
                    {
                        Username = username,
                        Password = password,
                        Date = date
                    });
                }
                stream.Close();
            }
            return logs;
        }


        // WriteListToText + DateTime.Now
        static void WriteToText(List<Logins> logs, int LevelOfEncrypt)
        {
            string date = DateTime.Now.ToString().Replace(':', '-');

            StreamWriter writer = new StreamWriter(date + " " + FileName + LevelOfEncrypt + " level.csv");

            foreach (Logins log in logs)
            {
                string lines =
                    $"{log.Username.Encrypt(LevelOfEncrypt)};" +
                    $"{log.Password.Encrypt(LevelOfEncrypt)};" +
                    $"{Convert.ToString(log.Date).Encrypt(LevelOfEncrypt)}";
                writer.WriteLine(lines);
            }
            writer.Close();
        }

        //  WriteListToBin + DateTime.Now
        static void WriteToBin(List<Logins> logs, int LevelOfEncrypt)
        {
            string date = DateTime.Now.ToString().Replace(':', '-');

            FileStream stream = new FileStream(date + " " + FileName + LevelOfEncrypt + " level.dat", FileMode.Append, FileAccess.Write);
            BinaryFormatter formatter = new BinaryFormatter();

            foreach (Logins log in logs)
            {
                LoginsBinary costumerBinary = new LoginsBinary()
                {
                    Username = Convert.ToString(log.Username).Encrypt(LevelOfEncrypt),
                    Password = Convert.ToString(log.Password).Encrypt(LevelOfEncrypt),
                    Date = Convert.ToString(log.Date).Encrypt(LevelOfEncrypt)
                };
                formatter.Serialize(stream, costumerBinary);
            }
            stream.Close();
        }


        // Conversion to Codec Class
        public static void EncryptionTextToText(int LevelOfSource, int LevelOfTarget)
        {
            List<Logins> logs = ReadFromText(LevelOfSource);
            if (logs.Count > 0)
            {
                WriteToText(logs, LevelOfTarget);
            }
            else
            {
                MessageBox.Show("Bitte kopieren Sie " + FileName + LevelOfSource + " level.csv in den Ordner");
            }
        }

        public static void EncryptionTextToBin(int LevelOfSource, int LevelOfTarget)
        {
            List<Logins> logs = ReadFromText(LevelOfSource);

            if (logs.Count > 0)
            {
                WriteToBin(logs, LevelOfTarget);
            }
            else
            {
                MessageBox.Show("Bitte kopieren Sie " + FileName + LevelOfSource + " level.csv in den Ordner");
            }
        }

        public static void EncryptionBinToBin(int LevelOfSource, int LevelOfTarget)
        {
            List<Logins> logs = ReadFromBin(LevelOfSource);
            if (logs.Count > 0)
            {
                WriteToBin(logs, LevelOfTarget);
            }
            else
            {
                MessageBox.Show("Bitte kopieren Sie " + FileName + LevelOfSource + " level.csv in den Ordner");
            }
        }

        public static void EncryptionBinToText(int LevelOfSource, int LevelOfTarget)
        {
            List<Logins> logs = ReadFromBin(LevelOfSource);

            if (logs.Count > 0)
            {
                WriteToText(logs, LevelOfTarget);
            }
            else
            {
                MessageBox.Show("Bitte kopieren Sie " + FileName + LevelOfTarget + " level.dat in den Ordner");
            }
        }

        [Serializable]
        class LoginsBinary
        {
            public string Username { get; set; }

            public string Password { get; set; }

            public string Date { get; set; }
        }
    }
}
