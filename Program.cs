using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;

namespace Exercise02
{
    class Program
    {
        static void Main(string[] args)
        {
            string xmlPath = "customers.xml";
            string key = "MySecretKey12345"; // 16 chars (128-bit key)

            if (!File.Exists(xmlPath))
            {
                Console.WriteLine("customers.xml not found.");
                return;
            }

            XElement doc = XElement.Load(xmlPath);

            foreach (var customer in doc.Elements("customer"))
            {
                var creditCardElement = customer.Element("creditcard");
                var passwordElement = customer.Element("password");

                if (creditCardElement != null && passwordElement != null)
                {
                    // Encrypt credit card
                    string creditCard = creditCardElement.Value;
                    string encryptedCard = EncryptString(creditCard, key);
                    creditCardElement.Value = encryptedCard;

                    // Hash password with salt
                    string password = passwordElement.Value;
                    string hashedPassword = HashPassword(password);
                    passwordElement.Value = hashedPassword;
                }
            }

            doc.Save("customers_protected.xml");
            Console.WriteLine("Protection applied and saved to customers_protected.xml");
        }

        static string EncryptString(string plainText, string key)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.GenerateIV();
                ICryptoTransform encryptor = aes.CreateEncryptor();

                using (MemoryStream ms = new MemoryStream())
                {
                    ms.Write(aes.IV, 0, aes.IV.Length); // store IV at beginning
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        byte[] inputBytes = Encoding.UTF8.GetBytes(plainText);
                        cs.Write(inputBytes, 0, inputBytes.Length);
                        cs.FlushFinalBlock();
                        return Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
        }

        static string DecryptString(string cipherText, string key)
        {
            byte[] fullCipher = Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {
                byte[] iv = new byte[16];
                byte[] cipher = new byte[fullCipher.Length - iv.Length];

                Array.Copy(fullCipher, iv, iv.Length);
                Array.Copy(fullCipher, iv.Length, cipher, 0, cipher.Length);

                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                ICryptoTransform decryptor = aes.CreateDecryptor();

                using (MemoryStream ms = new MemoryStream(cipher))
                using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (StreamReader reader = new StreamReader(cs))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        static string HashPassword(string password)
        {
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(32);

            byte[] hashBytes = new byte[48];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 32);

            return Convert.ToBase64String(hashBytes);
        }
    }
}
