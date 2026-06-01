using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Storage;
using DailyFoodSetApp.Services;
using Microsoft.Maui.Devices.Sensors;

namespace DailyFoodSetApp.Views;

public partial class SettingsPage : ContentPage
{
    private const string ThemeIndexKey = "AppThemeIndex";

    public SettingsPage()
    {
        InitializeComponent();

        ThemePicker.SelectedIndex = Preferences.Default.Get(ThemeIndexKey, 0);
        FontSizeSlider.Value = AccessibilityService.CurrentFontScale;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        DailyFoodSetApp.Services.AccessibilityService.ApplyFontScale(this);
    }

    private void OnThemeChanged(object sender, EventArgs e)
    {
        if (Microsoft.Maui.Controls.Application.Current == null) return;

        int selectedIndex = ThemePicker.SelectedIndex;
        Preferences.Default.Set(ThemeIndexKey, selectedIndex);

        Microsoft.Maui.Controls.Application.Current.UserAppTheme = selectedIndex switch
        {
            1 => AppTheme.Light,
            2 => AppTheme.Dark,
            _ => AppTheme.Unspecified
        };

        SemanticScreenReader.Announce("Theme has been updated.");
    }

    private void OnFontSizeChanged(object sender, ValueChangedEventArgs e)
    {
        AccessibilityService.CurrentFontScale = e.NewValue;
        AccessibilityService.ApplyFontScale(this);
    }

    private void OnVibrateClicked(object sender, EventArgs e)
    {
        try
        {
            Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(500));
            HapticFeedback.Default.Perform(HapticFeedbackType.LongPress);
            SemanticScreenReader.Announce("Device vibrated.");
        }
        catch (FeatureNotSupportedException)
        {
            DisplayAlert("Oops!", "Your device doesn't seem to support vibration.", "Okay");
        }
        catch (Exception ex)
        {
            DisplayAlert("Oops!", $"Something went wrong: {ex.Message}", "Okay");
        }
    }
}