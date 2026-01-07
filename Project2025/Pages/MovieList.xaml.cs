using Project2025.Classes;
using System.Collections.ObjectModel;

namespace Project2025.Pages;

public partial class MovieList : ContentPage
{
    private readonly List<Movie> allMovies = new();
    private readonly ObservableCollection<Movie> shownMovies = new();

    private readonly List<string> selectedGenres = new();
    private readonly List<string> selectedDirectors = new();

    int yearSort = 0;     
    int ratingSort = 0;  
    string lastSortPressed = "";

    public MovieList()
    {
        InitializeComponent();
        MovieListView.ItemsSource = shownMovies;
    }

    //showing movies list
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (allMovies.Count > 0)
            return;

        var service = new MovieDownloading();
        var movies = await service.GetMoviesAsync();

        allMovies.Clear();
        allMovies.AddRange(movies);

        RefreshShownMovies(allMovies);

        CreateGenreCheckboxesFromData();
        CreateDirectorCheckboxesFromData();
    }

    //search
    private void OnSearchChanged(object sender, TextChangedEventArgs e)
    {
        ApplyFiltersAndSort(e.NewTextValue ?? "");
    }

    //sort by year
    private void OnSortByYear(object sender, EventArgs e)
    {
        yearSort = yearSort == 1 ? -1 : 1;
        lastSortPressed = "Year";
        YearSortButton.Text = yearSort == 1 ? "Year UP" : "Year DOWN";
        ApplyFiltersAndSort(SearchBarControl.Text);
    }

    //sort by rating
    private void OnSortByRating(object sender, EventArgs e)
    {
        ratingSort = ratingSort == 1 ? -1 : 1;
        lastSortPressed = "Rating";
        RatingSortButton.Text = ratingSort == 1 ? "Rating UP" : "Rating DOWN";
        ApplyFiltersAndSort(SearchBarControl.Text);
    }

    private void OnToggleGenrePanel(object sender, EventArgs e)
    {
        GenrePanel.IsVisible = !GenrePanel.IsVisible;
    }

    private void OnToggleDirectorPanel(object sender, EventArgs e)
    {
        DirectorPanel.IsVisible = !DirectorPanel.IsVisible;
    }

    //showing checkbox with genres
    private void CreateGenreCheckboxesFromData()
    {
        GenreCheckboxContainer.Children.Clear();

        if (allMovies.Count == 0)
            return;

        HashSet<string> genreData = new();

        foreach (var movie in allMovies)
        {
            if (movie.genre == null)
                continue;

            foreach (var g in movie.genre)
            {
                if (!string.IsNullOrWhiteSpace(g))
                    genreData.Add(g);
            }
        }

        foreach (var g in genreData.OrderBy(g => g))
            GenreCheckboxContainer.Children.Add(CreateCheckboxRow(g, OnGenreChecked));
    }

    //if any genre was selected
    private void OnGenreChecked(object sender, CheckedChangedEventArgs e)
    {
        var genre = (string)((CheckBox)sender).BindingContext;

        if (e.Value)
            selectedGenres.Add(genre);
        else
            selectedGenres.Remove(genre);

        ApplyFiltersAndSort(SearchBarControl.Text);
    }

    //showing checkbox with directors
    private void CreateDirectorCheckboxesFromData()
    {
        DirectorCheckboxContainer.Children.Clear();

        var directors = allMovies
            .Where(m => !string.IsNullOrWhiteSpace(m.director))
            .Select(m => m.director)
            .Distinct()
            .OrderBy(d => d);

        foreach (var d in directors)
            DirectorCheckboxContainer.Children.Add(CreateCheckboxRow(d, OnDirectorChecked));
    }

    //if any director was selected
    private void OnDirectorChecked(object sender, CheckedChangedEventArgs e)
    {
        var director = (string)((CheckBox)sender).BindingContext;

        if (e.Value)
            selectedDirectors.Add(director);
        else
            selectedDirectors.Remove(director);

        ApplyFiltersAndSort(SearchBarControl.Text);
    }

    private View CreateCheckboxRow(string text, EventHandler<CheckedChangedEventArgs> handler)
    {
        var cb = new CheckBox { BindingContext = text };
        cb.CheckedChanged += handler;

        var label = new Label
        {
            Text = text,
            VerticalTextAlignment = TextAlignment.Center
        };
        return new HorizontalStackLayout
        {
            Spacing = 6,
            Padding = new Thickness(6, 0),
            Children = { cb, label }
        };
    }

    //filters and sorting
    private void ApplyFiltersAndSort(string search)
    {
        search = (search ?? "").Trim().ToLower();

        IEnumerable<Movie> result = allMovies;

        if (!string.IsNullOrWhiteSpace(search))
            result = result.Where(m =>
                !string.IsNullOrWhiteSpace(m.title) &&
                m.title.ToLower().Contains(search));

        if (selectedGenres.Any())
            result = result.Where(m =>
                m.genre != null &&
                m.genre.Any(g => selectedGenres.Contains(g)));

        if (selectedDirectors.Any())
            result = result.Where(m =>
                !string.IsNullOrWhiteSpace(m.director) &&
                selectedDirectors.Contains(m.director));

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

    //refreshing
    private void RefreshShownMovies(IEnumerable<Movie> movies)
    {
        shownMovies.Clear();
        foreach (var m in movies)
            shownMovies.Add(m);
    }

    private async void OnMovieTap(object sender, EventArgs e)
    {
        if (sender is not Label label)
            return;

        if (label.BindingContext is not Movie movie)
            return;

        await Navigation.PushAsync(new MovieDetails(movie));
    }
}