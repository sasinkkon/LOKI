using System;
using System.ComponentModel;
using System.IO;
using System.Text.Json;
using System.Text;
using System.Runtime.CompilerServices;
using ObjectOriented_Template.Components.Pages;

namespace SaveLoad
{


    public class StorageSaving
    {


        public static void Save(string user, string bio)

        {

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










