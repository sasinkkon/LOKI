using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SaveLoad
{

    public class StorageLoading
    {
        public static string StorageJsonString;

        // Load method uses UserData Class as its data structure and it can be called to "Load" data from encrypted .Json files.
        public static string[] Load(string user)
        {
            string directoryPath = AppDomain.CurrentDomain.BaseDirectory;
            string mainFolder = Path.Combine(directoryPath, "Users");
            string userFolderPath = Path.Combine(mainFolder, user);
            string dataStorageFilePath = Path.Combine(userFolderPath, "datastorage.json");

            if (!File.Exists(dataStorageFilePath))
                return Array.Empty<string>();

            string dataStorageFile = File.ReadAllText(dataStorageFilePath);


            DataStorageSplitter userStorage = JsonSerializer.Deserialize<DataStorageSplitter>(dataStorageFile);

            if (userStorage == null || string.IsNullOrEmpty(userStorage.Bio))
                return Array.Empty<string>();

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










