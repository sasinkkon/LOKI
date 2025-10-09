using System;
using System.ComponentModel;
using System.IO;
using System.Text.Json;
using System.Text;
using System.Runtime.CompilerServices;

namespace SaveLoad
{


    public class StorageLoading
    {
        //Load method uses UserData Class as its data structure and it can be called to "Load" data from encrypted .Json files.
        public static DataStorage Load(string user)

        {
            

            //PATH TO USER FILES: repos\jsontestbench\jsontestbench\bin\Debug\net9.0\Users
            string directoryPath = @AppDomain.CurrentDomain.BaseDirectory;
            string mainFolder = @$"{directoryPath}\Users";
            string newSubfolderName = user; // Define folder name from the given username
            string newFolderPath = Path.Combine(mainFolder, newSubfolderName);


           
            string dataStorageFile = File.ReadAllText(@$"{mainFolder}\{user}\datastorage.json");  
            DataStorage StringifiedUserStorage = JsonSerializer.Deserialize<DataStorage>(dataStorageFile);
            

            //Returns UserData Class in Json deserialized Class form, aka. normal Class form where data can be used easily in C# functions.
            return StringifiedUserStorage;

        }

          
        }
    }










