using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;
using DailyFoodSetApp.Models;

namespace DailyFoodSetApp.Services;

public static class FoodService
{
    private static readonly HttpClient HttpClient = new() { Timeout = TimeSpan.FromSeconds(15) };
    private static List<FoodItem>? _cachedFoods;

    public static async Task<List<FoodItem>> GetAllFoodsAsync()
    {
        if (_cachedFoods != null && _cachedFoods.Any())
            return _cachedFoods;

        _cachedFoods = new List<FoodItem>();

        if (MockApiConfig.IsConfigured && !string.IsNullOrEmpty(MockApiConfig.EndpointUrl))
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
            catch (Exception)
            {
            }
        }

        try
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync("Food.json");
            using var reader = new StreamReader(stream);
            var jsonContent = await reader.ReadToEndAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var localItems = JsonSerializer.Deserialize<List<FoodItem>>(jsonContent, options);

            if (localItems != null)
            {
                _cachedFoods = localItems;
            }
        }
        catch (Exception)
        {
        }

        return _cachedFoods ?? new List<FoodItem>();
    }

    public static async Task<bool> AddFoodToApiAsync(FoodItem newItem)
    {
        if (!MockApiConfig.IsConfigured) return false;

        try
        {
            var response = await HttpClient.PostAsJsonAsync(MockApiConfig.EndpointUrl, newItem);

            if (response.IsSuccessStatusCode)
            {
                var createdItem = await response.Content.ReadFromJsonAsync<FoodItem>();
                if (createdItem != null && _cachedFoods != null)
                {
                    _cachedFoods.Add(createdItem);
                }
                return true;
            }
            return false;
        }
        catch (Exception)
        {
            return false;
        }
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

    public static async Task<List<FoodItem>> GenerateMealPlanAsync(int targetCalories, string preferredSpiciness, string preferredSweetness)
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
                f.Calories <= maxCalories).ToList();

            if (category.Equals("Drink", StringComparison.OrdinalIgnoreCase))
            {
                candidates = candidates.Where(f => f.Sweetness.Equals(preferredSweetness, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            else
            {
                candidates = candidates.Where(f => f.Spiciness.Equals(preferredSpiciness, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (!candidates.Any())
            {
                return new FoodItem
                {
                    Id = Guid.NewGuid().ToString("N"),
                    Name = "No match found",
                    Category = category,
                    Calories = 0,
                    Spiciness = "-",
                    Sweetness = "-",
                    Description = "We couldn't find anything matching your taste."
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

    public static async Task<bool> DeleteFoodFromApiAsync(string id)
    {
        if (!MockApiConfig.IsConfigured || string.IsNullOrEmpty(id))
            return false;

        try
        {
            var response = await HttpClient.DeleteAsync($"{MockApiConfig.EndpointUrl.TrimEnd('/')}/{id}");

            if (response.IsSuccessStatusCode)
            {
                if (_cachedFoods != null)
                {
                    var target = _cachedFoods.FirstOrDefault(f => f.Id == id);
                    if (target != null)
                    {
                        _cachedFoods.Remove(target);
                    }
                }
                return true;
            }
            return false;
        }
        catch (Exception)
        {
            return false;
        }
    }
}