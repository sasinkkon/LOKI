using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SaveLoad
{
    //This class and its functions can be called to retreave data into class object from json.
    public class StorageLoading
    {   
        public static string StorageJsonString;

        // Load method uses StorageData Class as its data structure and it can be called to "Load" data from encrypted .Json files.
        // In StorageData, data itself is stored as a one string for each of the Classes variables. So for an example all of the users "Bio" data is in one string and handled from that.
        public static string[] Load(string user)
        {

            //Here we assert varialbes for retreaving and using the right path in users computer filedirectory.
            string directoryPath = AppDomain.CurrentDomain.BaseDirectory;
            string mainFolder = Path.Combine(directoryPath, "Users");
            string userFolderPath = Path.Combine(mainFolder, user);
            string dataStorageFilePath = Path.Combine(userFolderPath, "datastorage.json");

            // If file does not exist, method creates only empty array which will not be used.
            if (!File.Exists(dataStorageFilePath))
                return Array.Empty<string>();

            // Reads JSON file into dataStorage variable
            string dataStorageFile = File.ReadAllText(dataStorageFilePath);

            // Uses DataStorageSplitter Class structure and functions to split dataStorageFile into string type list objects.
            DataStorageSplitter userStorage = JsonSerializer.Deserialize<DataStorageSplitter>(dataStorageFile);

            // If array or string are null, returns empty array which will not be used.
            if (userStorage == null || string.IsNullOrEmpty(userStorage.Bio))
                return Array.Empty<string>();

            // Returns string type array, with objects from StorageData keys (eg. Bio) value pair string - - -> this array can be used then for other functions.
            return userStorage.Bio.Split(new[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
        }
    }

    public class DataStorageSplitter
    {
        public string Bio { get; set; }

        [JsonIgnore]
        public string[] Dialogs
        {
            get
            {
                return string.IsNullOrEmpty(Bio)
                    ? Array.Empty<string>()
                    : Bio.Split(new[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
            }
        }
    }

}










