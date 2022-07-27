using System;
using System.Security.Cryptography;
using System.Text;

namespace Common
{
    public sealed class Encryption
    {
        public static String Encrypt_Decrypt(String pText, String pPhrase, String pSalt, Int16 pIterations, Int32 pKeySize, Boolean pEncrypt)
        {
            String rText = string.Empty;
            try
            {
                Byte[] initVector = Encoding.ASCII.GetBytes("ENCRYPTIONVECTOR");
                Byte[] saltValue = Encoding.ASCII.GetBytes(pSalt);
                System.Text.Encoding enc = System.Text.Encoding.ASCII;
                Rfc2898DeriveBytes bPassword = new Rfc2898DeriveBytes(pPhrase, saltValue, pIterations);
                Byte[] keyBytes = bPassword.GetBytes(pKeySize / 8);
                RijndaelManaged symmetricKey = new RijndaelManaged();
                symmetricKey.Mode = CipherMode.CBC;
                if (pEncrypt)
                {
                    Byte[] sText = Encoding.Default.GetBytes(pText);
                    ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVector);
                    System.IO.MemoryStream mStream = new System.IO.MemoryStream();
                    CryptoStream cryptStream = new CryptoStream(mStream, encryptor, CryptoStreamMode.Write);

                    cryptStream.Write(sText, 0, sText.Length);
                    cryptStream.FlushFinalBlock();

                    Byte[] cipherText = mStream.ToArray();
                    mStream.Close();
                    cryptStream.Close();
                    rText = Convert.ToBase64String(cipherText);
                }
                else
                {
                    Byte[] sText = Convert.FromBase64String(pText);
                    ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVector);
                    System.IO.MemoryStream mStream = new System.IO.MemoryStream(sText);
                    CryptoStream cryptStream = new CryptoStream(mStream, decryptor, CryptoStreamMode.Read);

                    Byte[] plainText = new Byte[sText.Length];
                    Int32 decByteCount = cryptStream.Read(plainText, 0, plainText.Length);

                    mStream.Close();
                    cryptStream.Close();
                    rText = Encoding.Default.GetString(plainText, 0, decByteCount);
                }
                // return rText;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return rText;
        }
    }

    public static class Common
    {
        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

    }

}
