using System;
using System.ComponentModel;
using System.IO;
using System.Text.Json;
using System.Text;
using System.Runtime.CompilerServices;
using ObjectOriented_Template.Components.Pages;

namespace SaveLoad
{

    //This class and its functions can be called to write data into datastorage json file.
    public class StorageSaving
    {
        // Save method takes two parameters, user and datastorage class object, which ever is being used.
        public static void Save(string user, string bio)

        {
            //Here we assert varialbes for retreaving and using the right path in users computer filedirectory.
            string directoryPath = @AppDomain.CurrentDomain.BaseDirectory;
            string mainFolder = @$"{directoryPath}\Users";
            string newSubfolderName = user; // Define folder name from the given username
            string newFolderPath = Path.Combine(mainFolder, newSubfolderName);
            string dataStoragePath = Path.Combine(newFolderPath, "datastorage.json");

            DataStorage dataStorageInstance = new DataStorage
            {
                Bio = bio
            };


            string storageDataJson = JsonSerializer.Serialize(dataStorageInstance, new JsonSerializerOptions { WriteIndented = true });

            File.WriteAllText(dataStoragePath, storageDataJson);


        }

          
        }
    }










