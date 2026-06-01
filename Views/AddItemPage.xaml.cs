using DailyFoodSetApp.Models;
using DailyFoodSetApp.Services;

namespace DailyFoodSetApp.Views;

public partial class AddItemPage : ContentPage
{
    public AddItemPage()
    {
        InitializeComponent();
    }

    private async void OnSubmitButtonClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(NameEntry.Text) || CategoryPicker.SelectedIndex < 0)
        {
            await DisplayAlert("Missing Details", "Please give your food a name and pick a category before saving.", "Got it");
            return;
        }

        int.TryParse(CaloriesEntry.Text, out int parsedCalories);

        var customizedItem = new FoodItem
        {
            Id = Guid.NewGuid().ToString("N"),
            Name = NameEntry.Text.Trim(),
            Calories = parsedCalories,
            Spiciness = SpicinessPicker.SelectedItem?.ToString() ?? "Not Spicy",
            Sweetness = SweetnessPicker.SelectedItem?.ToString() ?? "Sugar Free",
            Category = CategoryPicker.SelectedItem?.ToString() ?? "Breakfast",
            Description = string.IsNullOrWhiteSpace(DescriptionEditor.Text) ? "No description provided." : DescriptionEditor.Text.Trim()
        };

        bool status = await FoodService.AddFoodToApiAsync(customizedItem);

        if (status)
        {
            await DisplayAlert("Success!", "Your new food has been saved successfully.", "Awesome");
            await Shell.Current.GoToAsync("..");
        }
        else
        {
            await DisplayAlert("Oops!", "We couldn't save your food right now. Please try again later.", "Okay");
        }
    }
}