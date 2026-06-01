using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http.Json;
using DailyFoodSetApp.Models;

namespace DailyFoodSetApp.Services;

public static class FoodService
{
    private static readonly HttpClient HttpClient = new() { Timeout = TimeSpan.FromSeconds(10) };
    private static List<FoodItem>? _cachedFoods;

    private static async Task<List<FoodItem>> GetAllFoodsAsync()
    {
        if (_cachedFoods != null && _cachedFoods.Any())
            return _cachedFoods;

        if (MockApiConfig.IsConfigured)
        {
            try
            {
                var remoteItems = await HttpClient.GetFromJsonAsync<List<FoodItem>>(MockApiConfig.EndpointUrl);
                if (remoteItems != null && remoteItems.Any())
                {
                    _cachedFoods = remoteItems;
                    return _cachedFoods;
                }
            }
            catch { }
        }

        _cachedFoods = new List<FoodItem>(FoodDataStore.SeedData);
        return _cachedFoods;
    }

    public static async Task<List<FoodItem>> SearchFoodsAsync(string query)
    {
        var allItems = await GetAllFoodsAsync();

        if (string.IsNullOrWhiteSpace(query))
            return allItems.OrderBy(f => f.Name).ToList();

        string normalized = query.Trim();
        return allItems.Where(f =>
            f.Name.Contains(normalized, StringComparison.OrdinalIgnoreCase) ||
            f.Category.Contains(normalized, StringComparison.OrdinalIgnoreCase))
            .OrderBy(f => f.Name).ToList();
    }

    public static async Task<FoodItem> GetFoodByIdAsync(string id)
    {
        var allItems = await GetAllFoodsAsync();
        return allItems.FirstOrDefault(f => f.Id == id);
    }

    public static async Task<List<FoodItem>> GenerateMealPlanAsync(int targetCalories, string preferredSpiciness)
    {
        var allItems = await GetAllFoodsAsync();
        var selectedPlan = new List<FoodItem>();
        var random = new Random();

        int breakfastCalLimit = (int)(targetCalories * 0.30) + 100;
        int lunchCalLimit = (int)(targetCalories * 0.35) + 150;
        int dinnerCalLimit = (int)(targetCalories * 0.30) + 150;
        int drinkCalLimit = (int)(targetCalories * 0.05) + 100;

        FoodItem FindMatch(string category, int maxCalories)
        {
            var candidates = allItems.Where(f =>
                f.Category.Equals(category, StringComparison.OrdinalIgnoreCase) &&
                f.Spiciness.Equals(preferredSpiciness, StringComparison.OrdinalIgnoreCase) &&
                f.Calories <= maxCalories).ToList();

            if (!candidates.Any())
            {
                return new FoodItem
                {
                    Name = "No matching food found",
                    Category = category,
                    Calories = 0,
                    Spiciness = "-",
                    Description = "No items match your specific calorie and spiciness requirements."
                };
            }

            return candidates[random.Next(candidates.Count)];
        }

        selectedPlan.Add(FindMatch("Breakfast", breakfastCalLimit));
        selectedPlan.Add(FindMatch("Lunch", lunchCalLimit));
        selectedPlan.Add(FindMatch("Dinner", dinnerCalLimit));
        selectedPlan.Add(FindMatch("Drink", drinkCalLimit));

        return selectedPlan;
    }
}