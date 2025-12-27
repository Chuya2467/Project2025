using Project2025.Classes;

namespace Project2025.Pages;

public partial class MovieList : ContentPage
{
    private MovieViewModel ViewModel => (MovieViewModel)BindingContext;

    public MovieList()
    {
        InitializeComponent();
        var service = new MovieDownloading();
        BindingContext = new MovieViewModel(service);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        
        if (ViewModel != null && !ViewModel.Movies.Any())
        {
            // LoadMoviesAsync безопасно
            try
            {
                await ViewModel.LoadMoviesAsync();
            }
            catch
            {
                // Игнорируем ошибки, чтобы Shell не ломался
            }
        }
    }

    private void OnSearchChanged(object sender, TextChangedEventArgs e)
    {
        ViewModel?.FilterMovies(e.NewTextValue);
    }
}