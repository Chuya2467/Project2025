using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Project2025.Classes
{
    public class MovieViewModel
    {
        private readonly MovieDownloading _service;

        public ObservableCollection<Movie> Movies { get; } = new();
        private List<Movie> _allMovies = new();

        public MovieViewModel(MovieDownloading service)
        {
            _service = service;
            if (_service != null)
                _ = LoadMoviesAsync();
        }

        public async Task LoadMoviesAsync()
        {
            if (_service == null)
                return;

            List<Movie> movies;
            try
            {
                movies = await _service.GetMoviesAsync();
            }
            catch
            {
                movies = new List<Movie>();
            }

            _allMovies = movies;

            Movies.Clear();
            foreach (var movie in movies)
                Movies.Add(movie);
        }

        public void FilterMovies(string searchText)
        {
            Movies.Clear();

            if (_allMovies == null || _allMovies.Count == 0)
                return;

            if (string.IsNullOrWhiteSpace(searchText))
            {
                foreach (var movie in _allMovies)
                    Movies.Add(movie);
                return;
            }

            var filtered = _allMovies
                .Where(m => m.title != null &&
                            m.title.Contains(searchText, StringComparison.OrdinalIgnoreCase));

            foreach (var movie in filtered)
                Movies.Add(movie);
        }
    }
}