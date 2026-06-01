using DailyFoodSetApp.Services;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
using Microsoft.Maui.Devices.Sensors;
using Microsoft.Maui.Storage;
using System;
using System.Threading.Tasks;

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

    private async void OnUploadDataClicked(object sender, EventArgs e)
    {
        UploadDataButton.IsEnabled = false;
        UploadProgressBar.IsVisible = true;
        UploadStatusLabel.IsVisible = true;
        UploadProgressBar.Progress = 0;
        UploadStatusLabel.Text = "Getting ready to backup...";

        var progressIndicator = new Progress<double>(value =>
        {
            UploadProgressBar.Progress = value;
            UploadStatusLabel.Text = $"Backing up: {(value * 100):F0}%";
        });

        int result = await FoodService.MigrateLocalDataToMockApiAsync(progressIndicator);

        if (result == -1)
        {
            UploadProgressBar.IsVisible = false;
            UploadStatusLabel.Text = "Your backup is already up to date!";
            await DisplayAlert("Cloud Sync", "Everything is already safely stored in the cloud. No need to backup again.", "Awesome");
        }
        else if (result > 0)
        {
            UploadStatusLabel.Text = $"Backed up {result} items successfully!";
            await DisplayAlert("Cloud Sync", "All done! Your food list is now safely backed up.", "Awesome");
        }
        else
        {
            UploadProgressBar.IsVisible = false;
            UploadStatusLabel.Text = "Backup failed. Please check your internet connection.";
            await DisplayAlert("Cloud Sync", "We couldn't backup your data right now. Please make sure you are connected to the internet.", "Okay");
            UploadDataButton.IsEnabled = true;
        }
    }
}