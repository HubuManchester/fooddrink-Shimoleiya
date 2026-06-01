using DailyFoodSetApp.Services;
using Microsoft.Maui.Devices.Sensors;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;

namespace DailyFoodSetApp.Views;

public partial class MealGeneratorPage : ContentPage
{
    private bool _isShakeEventBusy = false;

    public MealGeneratorPage()
    {
        InitializeComponent();
        SpicinessPicker.SelectedIndex = 0;
        SweetnessPicker.SelectedIndex = 0;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        DailyFoodSetApp.Services.AccessibilityService.ApplyFontScale(this);

        if (Accelerometer.Default.IsSupported)
        {
            Accelerometer.Default.ShakeDetected += Accelerometer_ShakeDetected;
            if (!Accelerometer.Default.IsMonitoring)
            {
                Accelerometer.Default.Start(SensorSpeed.Game);
            }
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        if (Accelerometer.Default.IsSupported && Accelerometer.Default.IsMonitoring)
        {
            Accelerometer.Default.Stop();
            Accelerometer.Default.ShakeDetected -= Accelerometer_ShakeDetected;
        }
    }

    private async void OnGenerateClicked(object sender, EventArgs e)
    {
        if (!int.TryParse(CaloriesEntry.Text, out int calories))
        {
            await DisplayAlert("Oops!", "Please enter a valid number for your calories.", "Okay");
            return;
        }

        if (calories <= 0)
        {
            await DisplayAlert("Wait a minute!", "The calories need to be more than zero.", "Okay");
            return;
        }

        string spiciness = SpicinessPicker.SelectedItem?.ToString() ?? "Not Spicy";
        string sweetness = SweetnessPicker.SelectedItem?.ToString() ?? "Sugar Free";

        var plan = await FoodService.GenerateMealPlanAsync(calories, spiciness, sweetness);

        MealPlanCollection.ItemsSource = plan;
        ResultsContainer.IsVisible = true;

        await Task.Delay(100);
        DailyFoodSetApp.Services.AccessibilityService.ApplyFontScale(this);
    }

    private async void OnShakeButtonClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Surprise Me!", "Shake your phone to discover a random tasty food!", "Awesome");
    }

    private void Accelerometer_ShakeDetected(object? sender, EventArgs e)
    {
        if (_isShakeEventBusy) return;
        _isShakeEventBusy = true;

        MainThread.BeginInvokeOnMainThread(async () =>
        {
            try
            {
                var allFoods = await FoodService.SearchFoodsAsync("");

                if (allFoods != null && allFoods.Any())
                {
                    var random = new Random();
                    var luckyFood = allFoods[random.Next(allFoods.Count)];

                    string message = $"Name: {luckyFood.Name}\n" +
                                     $"Category: {luckyFood.Category}\n" +
                                     $"Calories: {luckyFood.Calories} kcal\n" +
                                     $"Spiciness: {luckyFood.Spiciness}\n" +
                                     $"Sweetness: {luckyFood.Sweetness}\n\n" +
                                     $"Description: {luckyFood.Description}";

                    HapticFeedback.Default.Perform(HapticFeedbackType.Click);

                    await DisplayAlert("Your Surprise Food!", message, "Looks yummy!");
                }
            }
            finally
            {
                _isShakeEventBusy = false;
            }
        });
    }
}