namespace DailyFoodSetApp.Services;

public static class MockApiConfig
{
    public static bool IsConfigured { get; set; } = true;

    public static string EndpointUrl { get; set; } = "https://6a1ad633bc2f94475492b48b.mockapi.io/foods";
}