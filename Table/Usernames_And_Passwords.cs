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
    public class Usernames_And_Passwords
    {
        public static string FileName = "UsernameAndPasswords ";

        public string usernames { get; set; }

        public string passwords { get; set; }

        public int? passwordNumbers
        {
            get
            {
                return Convert.ToInt32(
                Convert.ToString(this.passwords[this.passwords.Length - 2]) +
                Convert.ToString(this.passwords[this.passwords.Length - 1]));
            }
            set { }
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            Usernames_And_Passwords objAsPart = obj as Usernames_And_Passwords;
            if (objAsPart == null) return false;
            else return Equals(objAsPart);
        }

        public bool Equals(Usernames_And_Passwords other)
        {
            if (other == null) return false;
            return (this.usernames.Equals(other.usernames) && this.passwords.Equals(other.passwords));
        }

        public override int GetHashCode()
        {
            int hashCode = -1717943133;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(usernames);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(passwords);
            return hashCode;
        }

        // Writing user input Creating a new record
        public static void WriteAddTextOrBin(Usernames_And_Passwords uap, int LevelOfAppEncryption, bool TextTrue_BinFalse)
        {
            if (TextTrue_BinFalse)
            {
                StreamWriter writer = new StreamWriter(FileName + LevelOfAppEncryption + " level.csv", true);
                string line =
                $"{uap.usernames.Encrypt(LevelOfAppEncryption)};" +
                $"{uap.passwords.Encrypt(LevelOfAppEncryption)}";
                writer.WriteLine(line);
                writer.Close();
            }

            else
            {
                FileStream stream = new FileStream(FileName + LevelOfAppEncryption + " level.dat", FileMode.Append, FileAccess.Write);
                BinaryFormatter formatter = new BinaryFormatter();
                Usernames_And_Passwords uapBinary = new Usernames_And_Passwords()
                {
                    usernames = uap.usernames.Encrypt(LevelOfAppEncryption),
                    passwords = uap.passwords.Encrypt(LevelOfAppEncryption)
                };
                formatter.Serialize(stream, uapBinary);
                stream.Close();
            }
        }

        // Reading List 
        public static List<Usernames_And_Passwords> ReadListTextOrBin(int LevelOfAppEncryption, bool TextTrue_BinFalse)
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
        static void WriteDeleteUsernameListToText(List<Usernames_And_Passwords> uaps, string DeleteUsername, int LevelOfEncrypt)
        {
            StreamWriter writer = new StreamWriter(FileName + LevelOfEncrypt + " level.csv");

            foreach (Usernames_And_Passwords uap in uaps)
            {
                if (uap.usernames != DeleteUsername)
                {
                    string line1 =
                    $"{uap.usernames.Encrypt(LevelOfEncrypt)};" +
                    $"{uap.passwords.Encrypt(LevelOfEncrypt)}";
                    writer.WriteLine(line1);
                }
            }
            writer.Close();
        }

        // Writing from List to BinaryList
        static void WriteDeleteUsernameListToBin(List<Usernames_And_Passwords> uaps, string DeleteUsername, int LevelOfEncrypt)
        {
            FileStream stream = new FileStream(FileName + LevelOfEncrypt + " level.dat", FileMode.Append, FileAccess.Write);

            BinaryFormatter formatter = new BinaryFormatter();

            foreach (Usernames_And_Passwords uap in uaps)
            {
                if (uap.usernames != DeleteUsername)
                {
                    Usernames_And_Passwords uapsBinary = new Usernames_And_Passwords()
                    {
                        usernames = uap.usernames,
                        passwords = uap.passwords
                    };
                    formatter.Serialize(stream, uapsBinary);
                }
            }
            stream.Close();
        }

        public static void WriteDeleteUsernameListTextOrBin(List<Usernames_And_Passwords> uaps, string DeleteUsername, int LevelOfAppEncryption, bool TextTrue_BinFalse)
        {
            if (TextTrue_BinFalse)
            {
                WriteDeleteUsernameListToText(uaps, DeleteUsername, LevelOfAppEncryption);
            }

            else
            {
                WriteDeleteUsernameListToBin(uaps, DeleteUsername, LevelOfAppEncryption);
            }
        }


        static List<Usernames_And_Passwords> ReadFromText(int LevelOfDecrypt)
        {
            List<Usernames_And_Passwords> uap = new List<Usernames_And_Passwords>();

            if (File.Exists(FileName + LevelOfDecrypt + " level.csv"))
            {
                StreamReader reader = new StreamReader(FileName + LevelOfDecrypt + " level.csv", Encoding.Default);
                string line = reader.ReadLine();
                while (line != null)
                {
                    string[] record = line.Split(';');
                    string username = record[0].Decrypt(LevelOfDecrypt);
                    string pasword = record[1].Decrypt(LevelOfDecrypt);
                    uap.Add(new Usernames_And_Passwords
                    {
                        usernames = username,
                        passwords = pasword
                    });
                    line = reader.ReadLine();
                }
                reader.Close();
            }

            return uap;
        }

        static List<Usernames_And_Passwords> ReadFromBin(int LevelOfDecrypt)
        {
            List<Usernames_And_Passwords> uap = new List<Usernames_And_Passwords>();
            if (File.Exists(FileName + LevelOfDecrypt + " level.dat"))
            {
                FileStream stream = new FileStream(FileName + LevelOfDecrypt + " level.dat", FileMode.Open, FileAccess.Read);
                BinaryFormatter formatter = new BinaryFormatter();
                while (stream.Position != stream.Length)
                {
                    Usernames_And_Passwords uapBinary = (Usernames_And_Passwords)formatter.Deserialize(stream);

                    string username = uapBinary.usernames.Decrypt(LevelOfDecrypt);
                    string password = uapBinary.passwords.Decrypt(LevelOfDecrypt);
                    uap.Add(new Usernames_And_Passwords
                    {
                        usernames = username,
                        passwords = password
                    });
                }
                stream.Close();
            }
            return uap;
        }

        // WriteListToText + DateTime.Now
        static void WriteToText(List<Usernames_And_Passwords> uaps, int LevelOfEncrypt)
        {
            string date = DateTime.Now.ToString().Replace(':', '-');

            StreamWriter writer = new StreamWriter(date + " " + FileName + LevelOfEncrypt + " level.csv");

            foreach (Usernames_And_Passwords uap in uaps)
            {
                string line1 =
                    $"{uap.usernames.Encrypt(LevelOfEncrypt)};" +
                    $"{uap.passwords.Encrypt(LevelOfEncrypt)}";
                writer.WriteLine(line1);
            }
            writer.Close();
        }

        //  WriteListToBin + DateTime.Now
        static void WriteToBin(List<Usernames_And_Passwords> uaps, int LevelOfEncrypt)
        {
            string date = DateTime.Now.ToString().Replace(':', '-');

            FileStream stream = new FileStream(date + " " + FileName + LevelOfEncrypt + " level.dat", FileMode.Append, FileAccess.Write);
            BinaryFormatter formatter = new BinaryFormatter();

            foreach (Usernames_And_Passwords uap in uaps)
            {
                Usernames_And_Passwords uapBinary = new Usernames_And_Passwords()
                {
                    usernames = uap.usernames.Encrypt(LevelOfEncrypt),
                    passwords = uap.passwords.Encrypt(LevelOfEncrypt)
                };
                formatter.Serialize(stream, uapBinary);
            }
            stream.Close();
        }


        // Conversion to Codec Class
        public static void EncryptionTextToText(int LevelOfSource, int LevelOfTarget)
        {
            List<Usernames_And_Passwords> uaps = ReadFromText(LevelOfSource);
            if (uaps.Count > 0)
            {
                WriteToText(uaps, LevelOfTarget);
            }
            else
            {
                MessageBox.Show("Bitte kopieren Sie " + FileName + LevelOfSource + " level.csv in den Ordner");
            }
        }

        public static void EncryptionTextToBin(int LevelOfSource, int LevelOfTarget)
        {
            List<Usernames_And_Passwords> uaps = ReadFromText(LevelOfSource);

            if (uaps.Count > 0)
            {
                WriteToBin(uaps, LevelOfTarget);
            }
            else
            {
                MessageBox.Show("Bitte kopieren Sie " + FileName + LevelOfSource + " level.csv in den Ordner");
            }
        }

        public static void EncryptionBinToBin(int LevelOfSource, int LevelOfTarget)
        {
            List<Usernames_And_Passwords> uaps = ReadFromBin(LevelOfSource);
            if (uaps.Count > 0)
            {
                WriteToBin(uaps, LevelOfTarget);
            }
            else
            {
                MessageBox.Show("Bitte kopieren Sie " + FileName + LevelOfSource + " level.csv in den Ordner");
            }
        }

        public static void EncryptionBinToText(int LevelOfSource, int LevelOfTarget)
        {
            List<Usernames_And_Passwords> uaps = ReadFromBin(LevelOfSource);

            if (uaps.Count > 0)
            {
                WriteToText(uaps, LevelOfTarget);
            }
            else
            {
                MessageBox.Show("Bitte kopieren Sie " + FileName + LevelOfSource + " level.csv in den Ordner");
            }
        }
    }
}
