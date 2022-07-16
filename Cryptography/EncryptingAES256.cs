using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Car_Dealership
{
    /// <summary>
    /// Data Multi Level Encryption and Decryption in AES-256
    /// </summary>
    public static class EncryptingAES256
    {
        private static string password = "X8ei0bZY2SC";

        /// <summary>
        /// Parameter is the encryption default value = 1 
        /// </summary>
        /// <param name="InputText"></param>
        /// <param name="LevelOfEncryption"></param>
        /// <returns></returns>
        public static string Encrypt(this string InputText, int LevelOfEncryption = 1)
        {
            if (LevelOfEncryption > 0)
            {
                for (int i = 0; i < LevelOfEncryption; i++)
                {
                    InputText = InputText.Encryption();
                }
                return InputText;
            }
            return InputText;
        }

        /// <summary>
        /// Parameter is the decryption level default value = 1
        /// </summary>
        /// <param name="InputText"></param>
        /// <param name="LevelOfDecryption"></param>
        /// <returns></returns>
        public static string Decrypt(this string InputText, int LevelOfDecryption = 1)
        {
            if (LevelOfDecryption > 0)
            {
                for (int i = 0; i < LevelOfDecryption; i++)
                {
                    InputText = InputText.Decryption();
                }
            }
            return InputText;
        }

        private static string Encryption(this string message)
        {
            SHA256 mySHA256 = SHA256Managed.Create();
            byte[] key = mySHA256.ComputeHash(Encoding.ASCII.GetBytes(password));

            byte[] iv = new byte[16] { 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 };

            string encrypted = EncryptString(message, key, iv);

            return encrypted;
        }

        private static string Decryption(this string encrypted)
        {
            SHA256 mySHA256 = SHA256Managed.Create();
            byte[] key = mySHA256.ComputeHash(Encoding.ASCII.GetBytes(password));

            byte[] iv = new byte[16] { 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 };

            string decrypted = DecryptString(encrypted, key, iv);

            return decrypted;
        }

        private static string EncryptString(string plainText, byte[] key, byte[] iv)
        {
            Aes encryptor = Aes.Create();
            encryptor.Mode = CipherMode.CBC;
            encryptor.Key = key;
            encryptor.IV = iv;

            MemoryStream memoryStream = new MemoryStream();
            ICryptoTransform aesEncryptor = encryptor.CreateEncryptor();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, aesEncryptor, CryptoStreamMode.Write);

            byte[] plainBytes = Encoding.ASCII.GetBytes(plainText);

            cryptoStream.Write(plainBytes, 0, plainBytes.Length);
            cryptoStream.FlushFinalBlock();

            byte[] cipherBytes = memoryStream.ToArray();

            memoryStream.Close();
            cryptoStream.Close();

            string cipherText = Convert.ToBase64String(cipherBytes, 0, cipherBytes.Length);

            return cipherText;
        }

        private static string DecryptString(string cipherText, byte[] key, byte[] iv)
        {
            Aes encryptor = Aes.Create();
            encryptor.Mode = CipherMode.CBC;
            encryptor.Key = key;
            encryptor.IV = iv;

            MemoryStream memoryStream = new MemoryStream();
            ICryptoTransform aesDecryptor = encryptor.CreateDecryptor();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, aesDecryptor, CryptoStreamMode.Write);

            string plainText = String.Empty;

            try
            {
                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                cryptoStream.Write(cipherBytes, 0, cipherBytes.Length);
                cryptoStream.FlushFinalBlock();
                byte[] plainBytes = memoryStream.ToArray();
                plainText = Encoding.ASCII.GetString(plainBytes, 0, plainBytes.Length);
            }
            finally
            {
                memoryStream.Close();
                cryptoStream.Close();
            }

            return plainText;
        }
    }
}
