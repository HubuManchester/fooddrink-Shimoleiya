using System.Collections.Generic;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace DailyFoodSetApp.Services;

public static class AccessibilityService
{
    private const string FontScaleKey = "AppFontScale";
    private static readonly Dictionary<int, double> OriginalFontSizes = new();

    public static double CurrentFontScale
    {
        get => Preferences.Default.Get(FontScaleKey, 1.0);
        set => Preferences.Default.Set(FontScaleKey, value);
    }

    public static void ApplyFontScale(Page page)
    {
        double scale = CurrentFontScale;

        var descendants = ((IVisualTreeElement)page).GetVisualTreeDescendants();

        foreach (var element in descendants)
        {
            if (element is VisualElement visualElement)
            {
                ScaleElement(visualElement, scale);
            }
        }
    }

    private static void ScaleElement(VisualElement element, double scale)
    {
        int hash = element.GetHashCode();

        if (element is Label label)
        {
            if (!OriginalFontSizes.ContainsKey(hash)) OriginalFontSizes[hash] = label.FontSize;
            label.FontSize = OriginalFontSizes[hash] * scale;
        }
        else if (element is Button button)
        {
            if (!OriginalFontSizes.ContainsKey(hash)) OriginalFontSizes[hash] = button.FontSize;
            button.FontSize = OriginalFontSizes[hash] * scale;
        }
        else if (element is Entry entry)
        {
            if (!OriginalFontSizes.ContainsKey(hash)) OriginalFontSizes[hash] = entry.FontSize;
            entry.FontSize = OriginalFontSizes[hash] * scale;
        }
        else if (element is SearchBar searchBar)
        {
            if (!OriginalFontSizes.ContainsKey(hash)) OriginalFontSizes[hash] = searchBar.FontSize;
            searchBar.FontSize = OriginalFontSizes[hash] * scale;
        }
    }
}