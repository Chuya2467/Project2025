using Project2025.Classes;
namespace Project2025.Pages;

public partial class MovieDetails : ContentPage
{
    private readonly Movie movie;
    public MovieDetails(Movie movie)
	{
		InitializeComponent();
		BindingContext = movie;
        this.movie = movie;
        
        TitleLabel.Text = movie.title;
        YearLabel.Text = $"Year: {movie.year}";
        GenreLabel.Text = string.Join(", ", movie.genre);
        EmojiLabel.Text = movie.emoji;
        RatingLabel.Text = $"IMDB: {movie.rating:0.0}";

        RecordViewed();
        UpdateFavButton();
    }

    //record if any movie was viewed
    private async void RecordViewed()
    {
        await HistoryData.AddAsync(new HistoryEntry
        {
            Title = movie.title,
            Year = movie.year,
            Genre = movie.genre,
            Emoji = movie.emoji,
            Action = "viewed",
            Timestamp = DateTime.Now
        });
    }

    private void UpdateFavButton()
    {
        bool isFavourite = false;

        foreach (var fav in FavouriteData.Favourites)
        {
            if (fav.title == movie.title && fav.year == movie.year)
            {
                isFavourite = true;
                break;
            }
        }

        if (isFavourite)
            FavButton.Text = "Remove from favourites";
        else
            FavButton.Text = "Add to favourites";
    }

    private async void OnToggleFavourite(object sender, EventArgs e)
    {
        bool added = await FavouriteData.ToggleAsync(movie);

        var entry = new HistoryEntry();
        entry.Title = movie.title;
        entry.Year = movie.year;
        entry.Genre = movie.genre;
        entry.Emoji = movie.emoji;

        if (added == true)
        {
            entry.Action = "favourited";
        }
        else
        {
            entry.Action = "unfavourited";
        }

        entry.Timestamp = DateTime.Now;

        await HistoryData.AddAsync(entry);

        UpdateFavButton();
    }
}