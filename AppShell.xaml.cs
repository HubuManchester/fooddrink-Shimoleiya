using DailyFoodSetApp.Views;

namespace DailyFoodSetApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(FoodDetailPage), typeof(FoodDetailPage));
    }
}
