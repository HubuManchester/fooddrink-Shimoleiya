using DailyFoodSetApp.Services;

namespace DailyFoodSetApp.Views;

public partial class MealGeneratorPage : ContentPage
{
    public MealGeneratorPage()
    {
        InitializeComponent();
        SpicinessPicker.SelectedIndex = 0;
    }

    private async void OnGenerateClicked(object sender, EventArgs e)
    {
        if (!int.TryParse(CaloriesEntry.Text, out int calories))
        {
            await DisplayAlert("Input Error", "Please enter a valid numeric value for calories.", "OK");
            return;
        }

        if (calories <= 0)
        {
            await DisplayAlert("Input Error", "Calories must be greater than zero.", "OK");
            return;
        }

        string spiciness = SpicinessPicker.SelectedItem?.ToString() ?? "Not Spicy";

        var plan = await FoodService.GenerateMealPlanAsync(calories, spiciness);

        MealPlanCollection.ItemsSource = plan;
        ResultsContainer.IsVisible = true;
    }
}