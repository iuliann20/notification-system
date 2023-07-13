using NotificationSystem.Common;
using NotificationSystem.Common.Interfaces;
using System.Security.Cryptography;

namespace NotificationSystem.Common.Implementation
{
    public class EncryptionService : IEncryptionService
    {
        public string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
            {
                throw new ArgumentNullException(nameof(plainText));
            }

            using Aes aes = Aes.Create();
            aes.Key = Constants.Key;
            aes.IV = Constants.Vector;
            ICryptoTransform transform = aes.CreateEncryptor(aes.Key, aes.IV);
            using MemoryStream memoryStream = new MemoryStream();
            using CryptoStream stream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write);
            using (StreamWriter streamWriter = new StreamWriter(stream))
            {
                streamWriter.Write(plainText);
            }

            return Convert.ToBase64String(memoryStream.ToArray());
        }

        public string Decrypt(string encryptedText)
        {
            if (string.IsNullOrEmpty(encryptedText))
            {
                throw new ArgumentNullException(nameof(encryptedText));
            }

            using Aes aes = Aes.Create();
            aes.Key = Constants.Key;
            aes.IV = Constants.Vector;
            ICryptoTransform transform = aes.CreateEncryptor(aes.Key, aes.IV);
            using MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(encryptedText));
            using CryptoStream stream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Read);
            using StreamReader streamReader = new StreamReader(stream);

            return streamReader.ReadToEnd();
        }
    }
}
