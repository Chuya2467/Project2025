using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

//this file handles history data

namespace Project2025.Classes
{
    public static class HistoryData
    {

        public static List<HistoryEntry> Entries { get; private set; } = new();

        private static string GetCurrentFilePath()
        {
            if (string.IsNullOrWhiteSpace(UserData.CurrentUserName))
                return null;

            return UserData.HistoryPathFor(UserData.CurrentUserName);
        }

        //works with json file
        public static async Task LoadAsync()
        {
            Entries.Clear();

            string filePath = GetCurrentFilePath();

            if (filePath == null || !File.Exists(filePath))
                return;

            string json = await File.ReadAllTextAsync(filePath);

            var list = JsonSerializer.Deserialize<List<HistoryEntry>>(json);

            if (list != null)
            {
                Entries.AddRange(list);
            }
        }

        //saves data to a file
        public static async Task SaveAsync()
        {
            string filePath = GetCurrentFilePath();

            if (filePath == null)
                return;

            string json = JsonSerializer.Serialize(Entries);

            await File.WriteAllTextAsync(filePath, json);
        }

        public static async Task ClearAsync()
        {
            Entries.Clear();
            await SaveAsync();
        }

        public static async Task AddAsync(HistoryEntry entry)
        {
            if (entry == null)
                return;

            Entries.Add(entry);
            await SaveAsync();
        }

        public static void ClearMemory()
        {
            Entries.Clear();
        }
    }
}
