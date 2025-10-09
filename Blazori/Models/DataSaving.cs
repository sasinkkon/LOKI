using System;
using System.ComponentModel;
using System.IO;
using System.Text.Json;
using System.Security.Cryptography;
using System.Text;

namespace SaveLoad
{

    public class DataSaving
    {
        //Save method saves inputs into UserData Class structure and handles encryption and filewriting to a local filesystem.
        public static void Save(string user, string password)

        {
            //If for some reason empty input isn't handled in frontend, creates the dummy "error guy" user data instead of breaking everything.
            if (user == null | password == null)
            {
                user = "ERROR GUY";
                password = "ERROR";
            } 
                

            //PATH TO USER FILES: repos\jsontestbench\jsontestbench\bin\Debug\net9.0\Users
            string directoryPath = @AppDomain.CurrentDomain.BaseDirectory;
            string mainFolder = @$"{directoryPath}\Users";

            string newSubfolderName = user; // Define folder name from the given username


            // Combine paths. Creates a folder with the username the user has given.
            string newFolderPath = Path.Combine(mainFolder, newSubfolderName);


            // Create the new folder if it doesn't exist
            Directory.CreateDirectory(newFolderPath);

            string keyLocation = @$"{newFolderPath}\key.txt";
            string ivLocation = @$"{newFolderPath}\iv.txt";


            // Define the user and storage JSON file paths
            string userFilePath = Path.Combine(newFolderPath, "userdata.json");
           

            // Create an object to serialize       
            UserData userDataInstance = new UserData
            {
                User = user,
                Pass = password
            };


            // Serialize the object to JSON
            string userDataJson = JsonSerializer.Serialize(userDataInstance, new JsonSerializerOptions { WriteIndented = true });
           

            // Encrypt the file
            SaveEncryptedToFile(userDataJson, userFilePath, newFolderPath);

          


        }


        //This method encrypts data and writes it into the userdata.json file as a base64 encoded string.
        private static void SaveEncryptedToFile(string userData, string userDataPath, string newFolderPath)
        {
            

            // Create Aes keys
            using (Aes myAes = Aes.Create())
            {
                
                // Encrypt the string to an array of bytes.
                byte[] encrypted = EncryptStringToBytes_Aes(userData, myAes.Key, myAes.IV);

                // Converts Encrypted bytes to Base64 type so that block size for text doesnt change, thus works because of this afterwords in decryption.
                string encryptedBase64 = Convert.ToBase64String(encrypted);

                
                // Writes text into file
                File.WriteAllText(userDataPath, encryptedBase64);
               


                // Save key and IV securely in .txt files which are stored in the given users directory
                File.WriteAllText(@$"{newFolderPath}\key.txt", Convert.ToBase64String(myAes.Key));
                File.WriteAllText(@$"{newFolderPath}\iv.txt", Convert.ToBase64String(myAes.IV));


            }
        }

        
        //EncryptStringToBytes_Aes encrypts data.
        public static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
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
                    }

                    encrypted = msEncrypt.ToArray();
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }

  
        }
    }










