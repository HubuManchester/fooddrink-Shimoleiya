using System;
using System.Text.Json.Serialization;

namespace DailyFoodSetApp.Models;

public class FoodItem
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = Guid.NewGuid().ToString("N");

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("category")]
    public string Category { get; set; } = string.Empty;

    [JsonPropertyName("calories")]
    public int Calories { get; set; }

    [JsonPropertyName("spiciness")]
    public string Spiciness { get; set; } = "Not Spicy";

    [JsonPropertyName("sweetness")]
    public string Sweetness { get; set; } = "Sugar Free";

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonIgnore]
    public string CaloriesLabel => $"{Calories} kcal";
}