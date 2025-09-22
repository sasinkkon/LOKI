using System;
using System.ComponentModel;
using System.IO;
using System.Text.Json;
using System.Security.Cryptography;
using System.Text;

namespace Program
{


    public class UserData
    {
        public string User { get; set; }
        public string Pass { get; set; }
    }

    class Saving
    {
        public static void Main()

        {
            // Prompts user for information to add to .json file
            Console.WriteLine("Create username");
            string user = Console.ReadLine();
            Console.WriteLine("Create password");
            string password = Console.ReadLine();
            Console.WriteLine("Tell about yourself");
            string bio = Console.ReadLine();

            //PATH TO USER FILES: repos\jsontestbench\jsontestbench\bin\Debug\net9.0\Users
            string directoryPath = @AppDomain.CurrentDomain.BaseDirectory;
            string mainFolder = @$"{directoryPath}\Users";

            string newSubfolderName = user; // Define folder name from the given username


            // Combine paths. Creates a folder with the username the user has given.
            string newFolderPath = Path.Combine(mainFolder, newSubfolderName);


            // Create the new folder if it doesn't exist
            Directory.CreateDirectory(newFolderPath);

            //LUO TÄHÄN TESTI POPUPILLE JOKA KERTOO ETTÄ USER ON JO OLEMASSA


            // Define the user and storage JSON file paths
            string userFilePath = Path.Combine(newFolderPath, "userdata.json");
            string storagesonFilePath = Path.Combine(newFolderPath, "datastorage.json");

            // Create an object to serialize
            //Creates instance for userdata for pulling the information 
            UserData userDataInstance = new UserData
            {
                User = user,
                Pass = password
            };

            var datastorage = new
            {
                Sleep = 6,
                Bio = bio
            };


            // Serialize the object to JSON
            string userDataJson = JsonSerializer.Serialize(userDataInstance, new JsonSerializerOptions { WriteIndented = true });
            string storageDataJson = JsonSerializer.Serialize(datastorage, new JsonSerializerOptions { WriteIndented = true });

            // Encrypt the file
            SaveEncryptedToFile(userDataJson,userFilePath,storageDataJson,storagesonFilePath);

            //Pull and decrypt the encrypted file, put it into the console.
            string encryptedUserFile = @$"{mainFolder}\{user}\userdata.json";
            Console.WriteLine("DECRYPTATTU JSON FILE JOKA VETÄSTY TIEDOSTOSTA: "+ PullEncryptedFromFile(encryptedUserFile)); 


        }
        //This method encrypts data and writes it into the userdata.json file as a base64 encoded string.
        private static void SaveEncryptedToFile(string userData, string userDataPath, string storageData, string storagePath)
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
                File.WriteAllText(storagePath, storageData);


                // Save key and IV securely in .txt files which are stored in programs current base directory
                File.WriteAllText("key.txt", Convert.ToBase64String(myAes.Key));
                File.WriteAllText("iv.txt", Convert.ToBase64String(myAes.IV));


            }
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









