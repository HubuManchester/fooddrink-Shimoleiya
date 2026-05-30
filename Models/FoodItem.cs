using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace DailyFoodSetApp.Models;

public class FoodItem
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = Guid.NewGuid().ToString("N");

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("category")]
    public string Category { get; set; } = string.Empty; // Breakfast, Lunch, Dinner, Drink

    [JsonPropertyName("calories")]
    public int Calories { get; set; }

    [JsonPropertyName("spiciness")]
    public string Spiciness { get; set; } = "Not Spicy"; // Not Spicy, Mild, Medium, Extra Spicy

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    // Read-only helper property for UI formatting
    [JsonIgnore]
    public string CaloriesLabel => $"{Calories} kcal";
}