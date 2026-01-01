using Project2025.Classes;

namespace Project2025.Pages;

public partial class HomePage
{
    public HomePage()
    {
        InitializeComponent();

        if (UserData.CurrentUserName != null && UserData.CurrentUserName.Trim() != "")
        {
            Shell.Current.GoToAsync("//MovieList");
        }
    }

    private async void OnLogin(object sender, EventArgs e)
    {
        string name = NameEntry.Text;

        if (name == null || name == "")
        {
            await DisplayAlert("Error", "Please enter your name", "OK");
            return;
        }

        UserData.Login(name);

        await Shell.Current.GoToAsync("//MovieList");
    }
}