using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Json;
using DailyFoodSetApp.Models;

namespace DailyFoodSetApp.Services;

public static class FoodService
{
    private static readonly HttpClient HttpClient = new() { Timeout = TimeSpan.FromSeconds(10) };
    private static List<FoodItem>? _cachedFoods;

    // Hardcoded English local data setup
    private static readonly List<FoodItem> LocalFoods = new()
    {
        new() { Name = "Berry Oatmeal Bowl", Category = "Breakfast", Calories = 320, Spiciness = "Not Spicy", Description = "Healthy and high protein breakfast option." },
        new() { Name = "Spicy Chicken Wrap", Category = "Breakfast", Calories = 450, Spiciness = "Mild", Description = "Energetic wrap to start your morning." },
        new() { Name = "Steamed Veggies & Brown Rice", Category = "Lunch", Calories = 480, Spiciness = "Not Spicy", Description = "Low-fat and clean diet meal for fitness." },
        new() { Name = "Mapo Tofu Rice Bowl", Category = "Lunch", Calories = 620, Spiciness = "Medium", Description = "Classic spicy Sichuan tofu served over hot rice." },
        new() { Name = "Pan-Seared Salmon with Asparagus", Category = "Dinner", Calories = 410, Spiciness = "Not Spicy", Description = "Premium fish fillet rich in Omega-3 fatty acids." },
        new() { Name = "Szechuan Poached Beef", Category = "Dinner", Calories = 750, Spiciness = "Extra Spicy", Description = "Very authentic, rich, and highly seasoned dish." },
        new() { Name = "Sugar-Free Green Tea", Category = "Drink", Calories = 0, Spiciness = "Not Spicy", Description = "Refreshing, iced, and absolutely clean taste." },
        new() { Name = "Sparkling Fruit Juice", Category = "Drink", Calories = 110, Spiciness = "Not Spicy", Description = "Slightly sweet and bubbly natural booster." }
    };

    // Fetches items from remote API if configured, otherwise falls back to local data smoothly
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
            catch
            {
                // Network error fallback to local data silently during demo
            }
        }

        // Default local source
        _cachedFoods = new List<FoodItem>(LocalFoods);
        return _cachedFoods;
    }

    // Filters data by search terms asynchronously
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

    // Recommends 4 items based on criteria asynchronously
    public static async Task<List<FoodItem>> GenerateMealPlanAsync(int targetCalories, string preferredSpiciness)
    {
        var allItems = await GetAllFoodsAsync();
        var selectedPlan = new List<FoodItem>();
        var random = new Random();

        // Helper delegate to handle dynamic filtering and fallback loops
        FoodItem PickItemByCategory(string category)
        {
            var options = allItems.Where(f => f.Category.Equals(category, StringComparison.OrdinalIgnoreCase) &&
                                              (f.Spiciness.Equals(preferredSpiciness, StringComparison.OrdinalIgnoreCase) || f.Spiciness.Equals("Not Spicy", StringComparison.OrdinalIgnoreCase))).ToList();

            // Fallback 1: Ignore spiciness check if zero matches found
            if (!options.Any())
                options = allItems.Where(f => f.Category.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();

            // Fallback 2: Generate placeholder if catalog lacks this specific category
            if (!options.Any())
                return new FoodItem { Name = "No matching food found", Category = category, Calories = 0 };

            return options[random.Next(options.Count)];
        }

        selectedPlan.Add(PickItemByCategory("Breakfast"));
        selectedPlan.Add(PickItemByCategory("Lunch"));
        selectedPlan.Add(PickItemByCategory("Dinner"));
        selectedPlan.Add(PickItemByCategory("Drink"));

        return selectedPlan;
    }
}