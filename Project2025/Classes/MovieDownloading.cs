using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

//saves json file from website provided

namespace Project2025.Classes
{
    public class MovieDownloading
    {
        private const string Url = "https://raw.githubusercontent.com/DonH-ITS/jsonfiles/refs/heads/main/moviesemoji.json";

        private static readonly string LocalPath =
            Path.Combine(FileSystem.AppDataDirectory, "Movies.json");

        public async Task<List<Movie>> GetMoviesAsync()
        {
            string json;

            if (File.Exists(LocalPath))
            {
                json = await File.ReadAllTextAsync(LocalPath);
            }
            else
            {
                using var http = new HttpClient();
                json = await http.GetStringAsync(Url);

                await File.WriteAllTextAsync(LocalPath, json);
            }

            return JsonSerializer.Deserialize<List<Movie>>(json)!;
        }
    }
}