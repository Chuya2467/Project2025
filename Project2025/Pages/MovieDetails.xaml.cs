using Project2025.Classes;
namespace Project2025.Pages;

public partial class MovieDetails : ContentPage
{
	public MovieDetails(Movie movie)
	{
		InitializeComponent();
		BindingContext = movie;

	}
}