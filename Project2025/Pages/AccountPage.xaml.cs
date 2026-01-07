using Project2025.Classes;
namespace Project2025.Pages;

    public partial class AccountPage : ContentPage
    {
        public AccountPage()
        {
            InitializeComponent();
        }

        //loading user data
        protected override async void OnAppearing()
        {
            if (string.IsNullOrWhiteSpace(UserData.CurrentUserName))
                WelcomeLabel.Text = "Welcome, unauthorized user";
            else
                WelcomeLabel.Text = $"Welcome, {UserData.CurrentUserName}";

            await FavouriteData.LoadAsync();
            await HistoryData.LoadAsync();

            RefreshFavs();
            RefreshHistory();
        }

        void RefreshFavs()
        {
            if (FavouriteData.Favourites == null || FavouriteData.Favourites.Count == 0) {
                EmptyLabel.IsVisible = true;
                FavListView.ItemsSource = null;
            }
            else
            {
                EmptyLabel.IsVisible = false;
                FavListView.ItemsSource = FavouriteData.Favourites;
            }
        }

        void RefreshHistory()
        {
            if (HistoryData.Entries == null || HistoryData.Entries.Count == 0) {
                HistoryListView.ItemsSource = null;
                return;
            }
            HistoryListView.ItemsSource = null;
            HistoryListView.ItemsSource = SortHistoryByNewest(HistoryData.Entries.ToList());
        }

        List<HistoryEntry> SortHistoryByNewest(List<HistoryEntry> list)
        {
            return list
                .OrderByDescending(x => x.Timestamp)
                .ToList();
        }

        private async void OnRemoveFavourite(object sender, EventArgs e)
        {
            Movie movie = (Movie)((Button)sender).BindingContext;
            if (movie == null)
                return;

            await FavouriteData.RemoveAsync(movie);

            await HistoryData.AddAsync(new HistoryEntry
            {
                Title = movie.title,
                Year = movie.year,
                Genre = movie.genre,
                Emoji = movie.emoji,
                Action = "unfavourited",
                Timestamp = DateTime.Now
            });

            RefreshFavs();
            RefreshHistory();
        }

        private async void OnLogout(object sender, EventArgs e)
        {
            FavouriteData.ClearMemory();
            HistoryData.ClearMemory();
            UserData.Logout();

            await Shell.Current.GoToAsync("//HomePage");
        }
    }