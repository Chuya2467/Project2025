using System.Text.Json;

namespace Project2025.Classes
{
    public static class UserData
    {
        private static readonly string usersFile = Path.Combine(FileSystem.AppDataDirectory, "users.json");

        public static string CurrentUserName { get; private set; } = "";

        private static List<string> users = new List<string>();

        public static void LoadUsers()
        {
            try
            {
                if (File.Exists(usersFile))
                {
                    string json = File.ReadAllText(usersFile);

                    var list = JsonSerializer.Deserialize<List<string>>(json);

                    if (list != null)
                        users = list;
                    else
                        users = new List<string>();
                }
                else
                {
                    users = new List<string>();
                }
            }

            catch
            {
                users = new List<string>();
            }
        }

        private static void SaveUsers()
        {
            string json = JsonSerializer.Serialize(users);
            File.WriteAllText(usersFile, json);
        }

        public static void Login(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return;

            username = username.Trim();

            LoadUsers();

            if (!users.Contains(username))
            {
                users.Add(username);
                SaveUsers();
            }

            CurrentUserName = username;
        }

        public static void Logout()
        {
            CurrentUserName = "";
        }

        public static string FavouritesPathFor(string user)
        {
            string safe = MakeSafe(user);
            return Path.Combine(FileSystem.AppDataDirectory, safe + "_favourites.json");
        }

        public static string HistoryPathFor(string user)
        {
            string safe = MakeSafe(user);
            return Path.Combine(FileSystem.AppDataDirectory, safe + "_history.json");
        }

        private static string MakeSafe(string s)
        {
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                s = s.Replace(c, '_');
            }
            return s;
        }
    }
}