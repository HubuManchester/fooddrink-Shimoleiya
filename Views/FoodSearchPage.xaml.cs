using DailyFoodSetApp.Services;

namespace DailyFoodSetApp.Views;

public partial class FoodSearchPage : ContentPage
{
    public FoodSearchPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        // Automatically request data when user navigates into page view context
        await LoadDataAsync("");
    }

    private async Task LoadDataAsync(string query)
    {
        FoodCollection.ItemsSource = await FoodService.SearchFoodsAsync(query);
    }

    private async void OnSearchPressed(object sender, EventArgs e) => await LoadDataAsync(FoodSearchBar.Text);
    private async void OnSearchTextChanged(object sender, TextChangedEventArgs e) => await LoadDataAsync(e.NewTextValue);
}