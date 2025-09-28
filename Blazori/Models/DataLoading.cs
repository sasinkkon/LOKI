using System;
using System.ComponentModel;
using System.IO;
using System.Text.Json;
using System.Security.Cryptography;
using System.Text;
using System.Runtime.CompilerServices;

namespace SaveLoad
{


    public class DataLoading
    {
        public static string Load(string user)

        {
            

            //PATH TO USER FILES: repos\jsontestbench\jsontestbench\bin\Debug\net9.0\Users
            string directoryPath = @AppDomain.CurrentDomain.BaseDirectory;
            string mainFolder = @$"{directoryPath}\Users";
            
            


            //Pull and decrypt the encrypted file, put it into the console.
            string encryptedUserFile = @$"{mainFolder}\{user}\userdata.json";
            string output = PullEncryptedFromFile(encryptedUserFile);
            return output;

        }
       
        
        //This method Pulls data from encrypted .json file to a form which can be read by human.
        public static string PullEncryptedFromFile(string encryptedUserFile)
        {

            // Read encrypted Base64 string
            string encryptedBase64 = File.ReadAllText(encryptedUserFile);
            byte[] encryptedBytes = Convert.FromBase64String(encryptedBase64);

            // Read key and IV
            byte[] key = Convert.FromBase64String(File.ReadAllText("key.txt"));
            byte[] iv = Convert.FromBase64String(File.ReadAllText("iv.txt"));

            // Decrypt
            string decrypted = DecryptStringFromBytes_Aes(encryptedBytes, key, iv);
            
            return decrypted;


        }


       public static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decryptor to perform the stream transform.
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
    }
}









