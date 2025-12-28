using Project2025.Classes;
using System.Collections.ObjectModel;

namespace Project2025.Pages;

public partial class MovieList : ContentPage
{
    private MovieViewModel ViewModel => (MovieViewModel)BindingContext;

    private List<Movie> allMovies = new();
    private readonly ObservableCollection<Movie> shownMovies = new();

    int yearSort = 0;
    int ratingSort = 0;
    string lastSortPressed = "";

    public MovieList()
    {
        InitializeComponent();

        var service = new MovieDownloading();
        BindingContext = new MovieViewModel(service);

        MovieListView.ItemsSource = shownMovies;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (!ViewModel.Movies.Any())
            await ViewModel.LoadMoviesAsync();

        allMovies = ViewModel.Movies.ToList();

        RefreshShownMovies(allMovies);
    }

    // ===== SEARCH =====
    private void OnSearchChanged(object sender, TextChangedEventArgs e)
    {
        ApplyFiltersAndSort(e.NewTextValue ?? "");
    }

    // ===== SORTING =====
    private void OnSortByYear(object sender, EventArgs e)
    {
        yearSort = yearSort == 1 ? -1 : 1;
        lastSortPressed = "Year";

        YearSortButton.Text = yearSort == 1 ? "Year UP" : "Year DOWN";

        ApplyFiltersAndSort(SearchBarControl.Text);
    }

    private void OnSortByRating(object sender, EventArgs e)
    {
        ratingSort = ratingSort == 1 ? -1 : 1;
        lastSortPressed = "Rating";

        RatingSortButton.Text = ratingSort == 1 ? "Rating UP" : "Rating DOWN";

        ApplyFiltersAndSort(SearchBarControl.Text);
    }

    // ===== FILTER + SORT CORE =====
    private void ApplyFiltersAndSort(string search)
    {
        search = search?.Trim().ToLower() ?? "";

        IEnumerable<Movie> result = allMovies;

        if (!string.IsNullOrWhiteSpace(search))
            result = result.Where(m => m.title.ToLower().Contains(search));

        if (lastSortPressed == "Year")
            result = yearSort == 1
                ? result.OrderBy(m => m.year)
                : result.OrderByDescending(m => m.year);

        if (lastSortPressed == "Rating")
            result = ratingSort == 1
                ? result.OrderBy(m => m.rating)
                : result.OrderByDescending(m => m.rating);

        RefreshShownMovies(result);
    }

    private void RefreshShownMovies(IEnumerable<Movie> movies)
    {
        shownMovies.Clear();
        foreach (var m in movies)
            shownMovies.Add(m);
    }
}