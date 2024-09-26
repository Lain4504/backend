using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace BackEnd.Service.ServiceImpl
{
    public class EncryptDecryptManager
    {
        // Đặt khóa 16 byte cho AES-128
        private static readonly byte[] key = Encoding.UTF8.GetBytes("y8+nSs/2X#`:<&aZ");


        public static string Encrypt(string text)
        {
            byte[] iv = new byte[16]; // IV mặc định 16 byte
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                using (ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                using (MemoryStream ms = new MemoryStream())
                using (CryptoStream cryptoStream = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                {
                    streamWriter.Write(text);
                    streamWriter.Flush(); // Đảm bảo tất cả dữ liệu được ghi vào stream
                    cryptoStream.FlushFinalBlock(); // Đảm bảo dữ liệu cuối cùng được mã hóa
                    array = ms.ToArray();
                }
            }

            return Convert.ToBase64String(array);
        }

        public static string Decrypt(string text)
        {
            byte[] iv = new byte[16]; // IV mặc định 16 byte
            byte[] buffer = Convert.FromBase64String(text);

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                using (ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                using (MemoryStream ms = new MemoryStream(buffer))
                using (CryptoStream cryptoStream = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (StreamReader sr = new StreamReader(cryptoStream))
                {
                    return sr.ReadToEnd();
                }
            }
        }
    }
}
