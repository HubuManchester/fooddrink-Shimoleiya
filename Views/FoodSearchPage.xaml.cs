using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using DailyFoodSetApp.Models;
using DailyFoodSetApp.Services;

namespace DailyFoodSetApp.Views;

public partial class FoodSearchPage : ContentPage
{
    private ObservableCollection<FoodItem> _foodItems = new();
    private List<FoodItem> _currentFilteredResults = new();
    private bool _isLoadingMore = false;

    private int _currentPage = 0;
    private const int PageSize = 8;

    private bool _isInitializing = true;

    public FoodSearchPage()
    {
        InitializeComponent();
        FoodCollection.ItemsSource = _foodItems;

        CategoryPicker.SelectedIndex = 0;
        SpicinessPicker.SelectedIndex = 0;
        CaloriesPicker.SelectedIndex = 0;

        _isInitializing = false;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadDataAsync(false);
        DailyFoodSetApp.Services.AccessibilityService.ApplyFontScale(this);
    }

    private async void OnFilterChanged(object sender, EventArgs e)
    {
        if (_isInitializing) return;
        await LoadDataAsync(false);
    }

    private async void OnSearchPressed(object sender, EventArgs e)
    {
        await LoadDataAsync(false);
    }

    private async Task LoadDataAsync(bool isLoadMore)
    {
        if (!isLoadMore)
        {
            _currentPage = 0;
            _foodItems.Clear();

            var query = FoodSearchBar.Text ?? string.Empty;
            var results = await FoodService.SearchFoodsAsync(query);

            var category = CategoryPicker.SelectedItem?.ToString();
            if (!string.IsNullOrEmpty(category) && category != "All")
            {
                results = results.Where(f => f.Category.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            var spiciness = SpicinessPicker.SelectedItem?.ToString();
            if (!string.IsNullOrEmpty(spiciness) && spiciness != "All")
            {
                results = results.Where(f => f.Spiciness.Equals(spiciness, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            var calRange = CaloriesPicker.SelectedItem?.ToString();
            if (!string.IsNullOrEmpty(calRange) && calRange != "All")
            {
                if (calRange == "0 - 300")
                    results = results.Where(f => f.Calories <= 300).ToList();
                else if (calRange == "301 - 600")
                    results = results.Where(f => f.Calories >= 301 && f.Calories <= 600).ToList();
                else if (calRange == "600+")
                    results = results.Where(f => f.Calories > 600).ToList();
            }

            _currentFilteredResults = results;
        }

        var itemsToAdd = _currentFilteredResults
                            .Skip(_currentPage * PageSize)
                            .Take(PageSize)
                            .ToList();

        foreach (var item in itemsToAdd)
        {
            _foodItems.Add(item);
        }

        if (itemsToAdd.Any())
        {
            _currentPage++;
        }
        await Task.Delay(100);
        DailyFoodSetApp.Services.AccessibilityService.ApplyFontScale(this);
    }

    private async void OnRefreshing(object sender, EventArgs e)
    {
        await LoadDataAsync(false);
        FoodRefreshView.IsRefreshing = false;
        SemanticScreenReader.Announce("List refreshed.");
    }

    private async void OnLoadMore(object sender, EventArgs e)
    {
        if (_isLoadingMore || _foodItems.Count >= _currentFilteredResults.Count) return;

        _isLoadingMore = true;

        await Task.Delay(500);

        await LoadDataAsync(true);

        _isLoadingMore = false;
    }

    private async void OnDetailsClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is string id)
        {
            await Shell.Current.GoToAsync($"{nameof(FoodDetailPage)}?id={id}");
        }
    }
}