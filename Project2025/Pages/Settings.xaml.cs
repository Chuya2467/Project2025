namespace Project2025.Pages;

public partial class Settings : ContentPage
{
        public static class SettingsStore
        {
            public static bool DarkThemeEnabled { get; set; }
        }

        public Settings()
        {
            InitializeComponent();
 
            ThemeSwitch.IsToggled = SettingsStore.DarkThemeEnabled;
        }

        private void OnThemeToggled(object sender, ToggledEventArgs e)
        {
            bool isDark = e.Value;

            if (isDark)
            {
                Application.Current.UserAppTheme = AppTheme.Dark;
            }

            else
            {
                Application.Current.UserAppTheme = AppTheme.Light;
            }
        }

}