using Project2025.Classes;
namespace Project2025.Pages;

public partial class MovieDetails : ContentPage
{
    private readonly Movie movie;
    public MovieDetails(Movie movie)
	{
		InitializeComponent();
		BindingContext = movie;
        //filling ui
        TitleLabel.Text = movie.title;
        YearLabel.Text = $"Year: {movie.year}";
        GenreLabel.Text = string.Join(", ", movie.genre);
        EmojiLabel.Text = movie.emoji;
        RatingLabel.Text = $"IMDB: {movie.rating:0.0}";

    }

}
