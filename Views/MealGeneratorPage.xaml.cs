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
        // Check input conversion validity
        if (!int.TryParse(CaloriesEntry.Text, out int calories))
        {
            await DisplayAlert("Alert", "Please enter a valid calorie value.", "OK");
            return;
        }

        string spiciness = SpicinessPicker.SelectedItem?.ToString() ?? "Not Spicy";

        // Await asynchronous data retrieval processing
        var plan = await FoodService.GenerateMealPlanAsync(calories, spiciness);

        MealPlanCollection.ItemsSource = plan;
        ResultsContainer.IsVisible = true;
    }
}