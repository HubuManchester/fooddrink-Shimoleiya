namespace DailyFoodSetApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            int savedTheme = Microsoft.Maui.Storage.Preferences.Default.Get("AppThemeIndex", 0);

            UserAppTheme = savedTheme switch
            {
                1 => AppTheme.Light,
                2 => AppTheme.Dark,
                _ => AppTheme.Unspecified
            };

            MainPage = new AppShell();
        }
    }
}