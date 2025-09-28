using System;


namespace Program
{


    public class UserInput(string user, string password, string bio)
    {
        public static string user;
        public static string password;
        public static string bio;
        public static void Save()

        {
            //// Prompts user for information to add to .json file
            //Console.WriteLine("Create username");
            //user = Console.ReadLine();
            //Console.WriteLine("Create password");
            //password = Console.ReadLine();
            //Console.WriteLine("Tell about yourself");
            //bio = Console.ReadLine();

            DataSaving.Save();
            



        }

        public static void Load()

        {
            //// Prompts user for information to add to .json file
            //Console.WriteLine("Create username");
            //user = Console.ReadLine();
            //Console.WriteLine("Create password");
            //password = Console.ReadLine();
            //Console.WriteLine("Tell about yourself");
            //bio = Console.ReadLine();

            
            DataLoading.Load();



        }
    }
}









