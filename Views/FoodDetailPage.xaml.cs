using System;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Media;
using DailyFoodSetApp.Models;
using DailyFoodSetApp.Services;

namespace DailyFoodSetApp.Views;

[QueryProperty(nameof(TargetFoodId), "id")]
public partial class FoodDetailPage : ContentPage
{
    private FoodItem? currentTargetItem;
    private CancellationTokenSource? _ttsCts;

    public string TargetFoodId
    {
        set => _ = FetchTargetNodeContextAsync(value);
    }

    public FoodDetailPage()
    {
        InitializeComponent();
    }

    private async Task FetchTargetNodeContextAsync(string id)
    {
        try
        {
            currentTargetItem = await FoodService.GetFoodByIdAsync(id);
            if (currentTargetItem != null)
            {
                NameDisplay.Text = currentTargetItem.Name;
                CategoryDisplay.Text = $"Category: {currentTargetItem.Category}";
                CaloriesDisplay.Text = currentTargetItem.CaloriesLabel;
                SpecsDisplay.Text = $"Spiciness: {currentTargetItem.Spiciness} | Sweetness: {currentTargetItem.Sweetness}";
                DescriptionDisplay.Text = string.IsNullOrWhiteSpace(currentTargetItem.Description)
                    ? "There are no details for this food yet."
                    : currentTargetItem.Description;
            }
            else
            {
                NameDisplay.Text = "Oops! We couldn't find this food.";
            }
        }
        catch (Exception)
        {
        }
    }

    private async void OnNarrateSummaryClicked(object sender, EventArgs e)
    {
        if (currentTargetItem == null) return;

        _ttsCts?.Cancel();
        _ttsCts = new CancellationTokenSource();

        string audioScript = $"{currentTargetItem.Name}, categorized under {currentTargetItem.Category}. It has {currentTargetItem.Calories} calories. Spiciness is {currentTargetItem.Spiciness} and sweetness is {currentTargetItem.Sweetness}.";

        try
        {
            await TextToSpeech.Default.SpeakAsync(audioScript, cancelToken: _ttsCts.Token);
        }
        catch (Exception)
        {
        }
    }

    private void OnStopNarratingClicked(object sender, EventArgs e)
    {
        _ttsCts?.Cancel();
    }

    private async void OnTerminateRecordClicked(object sender, EventArgs e)
    {
        if (currentTargetItem == null) return;

        bool approval = await DisplayAlert("Delete Food?", "Are you sure you want to remove this food? This cannot be undone.", "Yes, remove it", "No, keep it");
        if (!approval) return;

        bool operationResult = await FoodService.DeleteFoodFromApiAsync(currentTargetItem.Id);

        if (operationResult)
        {
            await DisplayAlert("Deleted", "The food has been removed from your list.", "Okay");
            await Shell.Current.GoToAsync("..");
        }
        else
        {
            await DisplayAlert("Oops!", "We couldn't delete this food right now. Please try again later.", "Okay");
        }
    }
}