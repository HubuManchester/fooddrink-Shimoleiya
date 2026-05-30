using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DailyFoodSetApp.Services;

public static class MockApiConfig
{
    public const string EndpointUrl = "";

    public static bool IsConfigured => !string.IsNullOrWhiteSpace(EndpointUrl);
}