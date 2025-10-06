using SaveLoad;

namespace CredidentalsManager
{

    public class CredidentalsManager()
    {
        
        public static bool isUserAccess(string userName, string password)
        {

            try
            {
                var LoadedUserData = DataLoading.Load(userName);

                    if (LoadedUserData.Pass == password)
                    {
                        return true; // User and pass ok
                    }
            } catch
            {
                return false; // Return false if user data not found
            }
            

            return false; // User not found
        }
    }
}

