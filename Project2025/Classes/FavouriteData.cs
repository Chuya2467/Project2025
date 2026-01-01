using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Project2025.Classes
{
    public static class FavouriteData
    {

        public static ObservableCollection<Movie> Favourites { get; } = new();

        public static async Task LoadAsync()
        {
            Favourites.Clear();

            var user = UserData.CurrentUserName;
            if (string.IsNullOrWhiteSpace(user))
                return;

            var path = UserData.FavouritesPathFor(user);
            if (!File.Exists(path))
                return;

            var json = await File.ReadAllTextAsync(path);
            var list = JsonSerializer.Deserialize<List<Movie>>(json);

            if (list != null)
                foreach (var m in list)
                    Favourites.Add(m);
        }

        public static async Task SaveAsync()
        {
            var user = UserData.CurrentUserName;
            if (string.IsNullOrWhiteSpace(user))
                return;

            var path = UserData.FavouritesPathFor(user);
            var json = JsonSerializer.Serialize(Favourites.ToList());
            await File.WriteAllTextAsync(path, json);
        }

        public static async Task<bool> ToggleAsync(Movie m)
        {
            if (m == null)
                return false;

            var existing = Favourites.FirstOrDefault(x => x.title == m.title && x.year == m.year);

            if (existing != null)
            {
                Favourites.Remove(existing);
                await SaveAsync();
                return false;
            }

            Favourites.Add(new Movie
            {
                title = m.title,
                year = m.year,
                genre = new List<string>(m.genre),
                director = m.director,
                rating = m.rating,
                emoji = m.emoji
            });

            await SaveAsync();
            return true;
        }

        public static async Task RemoveAsync(Movie movie)
        {
            if (movie == null)
                return;

            Movie movieToRemove = null;

            foreach (var item in Favourites)
            {
                if (item.title == movie.title && item.year == movie.year)
                {
                    movieToRemove = item;
                    break;
                }
            }

            if (movieToRemove != null)
            {
                Favourites.Remove(movieToRemove);

                await SaveAsync();
            }
        }

        public static void ClearMemory()
        {
            Favourites.Clear();
        }
    }
}
