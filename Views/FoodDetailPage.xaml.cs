using DailyFoodSetApp.Models;
using DailyFoodSetApp.Services;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Media;
using System;
using System.Threading;
using System.Xml;

namespace DailyFoodSetApp.Views;

[QueryProperty(nameof(FoodId), "id")]
public partial class FoodDetailPage : ContentPage
{
    private CancellationTokenSource _speechTokenSource;
    private FoodItem _currentFood;

    public string FoodId
    {
        set => LoadFoodItem(value);
    }

    public FoodDetailPage()
    {
        InitializeComponent();
    }

    private async void LoadFoodItem(string id)
    {
        _currentFood = await FoodService.GetFoodByIdAsync(id);

        if (_currentFood != null)
        {
            NameLabel.Text = _currentFood.Name;
            CategoryLabel.Text = _currentFood.Category;
            CaloriesLabel.Text = _currentFood.CaloriesLabel;
            SpicinessLabel.Text = _currentFood.Spiciness;
            DescriptionLabel.Text = _currentFood.Description;
        }
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        DailyFoodSetApp.Services.AccessibilityService.ApplyFontScale(this);
    }

    private async void OnReadClicked(object sender, EventArgs e)
    {
        if (_currentFood == null) return;

        StopReading();

        _speechTokenSource = new CancellationTokenSource();

        string textToRead = $"{_currentFood.Name}. Category: {_currentFood.Category}. Calories: {_currentFood.Calories}. Spiciness level: {_currentFood.Spiciness}. Description: {_currentFood.Description}";

        try
        {
            await TextToSpeech.Default.SpeakAsync(textToRead, cancelToken: _speechTokenSource.Token);
        }
        catch (OperationCanceledException)
        {
        }
    }

    private async void OnStopReadClicked(object sender, EventArgs e)
    {
        StopReading();

        SemanticScreenReader.Announce("Reading stopped explicitly.");

        if (sender is Button btn)
        {
            string originalText = btn.Text;
            btn.Text = "Stopped!";

            await Task.Delay(1000);

            btn.Text = originalText;
        }
    }

    private void StopReading()
    {
        if (_speechTokenSource != null && !_speechTokenSource.IsCancellationRequested)
        {
            _speechTokenSource.Cancel();
            _speechTokenSource.Dispose();
            _speechTokenSource = null;
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        StopReading();
    }
}