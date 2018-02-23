using System;
using System.IO;
using System.Numerics;
using System.Security.Cryptography;

namespace Kros.TroubleShooterCommon
{
    /// <summary>
    /// symetric cryptology used to encrypt sources sent frtom server to client
    /// </summary>
    public class AesHandler
    {
        /// <summary>
        /// encrypts mesage using key
        /// </summary>
        /// <param name="plainText"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
        public static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            byte[] encrypted;
            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = CreateKey(Key);
                aesAlg.IV = AesSalt;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {

                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }


            // Return the encrypted bytes from the memory stream.
            return encrypted;

        }

        /// <summary>
        /// decrypts message using key
        /// </summary>
        /// <param name="cipherText"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
        public static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = CreateKey(Key);
                aesAlg.IV = AesSalt;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }

        /// <summary>
        /// salt used here
        /// </summary>
        private static byte[] AesSalt = BigInteger.Parse("01234567891011121314151617181920", System.Globalization.NumberStyles.HexNumber).ToByteArray();

        /// <summary>
        /// extends key to 256 bytes.. 
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        private static byte[] CreateKey(byte[] password)
        {
            using (SHA256 sha = SHA256.Create())
            {
                return sha.ComputeHash(password);
            }

        }
    }
}