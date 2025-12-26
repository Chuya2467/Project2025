namespace Project2025.Pages;

public partial class MovieList : ContentPage
{
	public MovieList()
	{
		InitializeComponent();
	}

    private void OnSearchChanged(object sender, TextChangedEventArgs e)
    {
        string text = e.NewTextValue;

        if (text == null)
            text = "";
    }
}
