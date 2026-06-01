using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Media;
using Microsoft.Maui.Devices.Sensors;
using Microsoft.Maui.Controls;
using Microsoft.Maui.ApplicationModel;

namespace DailyFoodSetApp.Views;

public partial class HardwarePage : ContentPage
{
    private bool _isFlashlightOn = false;
    private ImageSource _capturedImageSource;
    private string _currentLocation = "Unknown Location";

    public HardwarePage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        DailyFoodSetApp.Services.AccessibilityService.ApplyFontScale(this);
    }

    private async void OnFlashlightClicked(object sender, EventArgs e)
    {
        try
        {
            if (_isFlashlightOn)
            {
                await Flashlight.Default.TurnOffAsync();
                _isFlashlightOn = false;
                if (sender is Button btn) btn.Text = "Flashlight: Off";
                SemanticScreenReader.Announce("Flashlight turned off.");
            }
            else
            {
                await Flashlight.Default.TurnOnAsync();
                _isFlashlightOn = true;
                if (sender is Button btn) btn.Text = "Flashlight: On";
                SemanticScreenReader.Announce("Flashlight turned on.");
            }
        }
        catch (FeatureNotSupportedException)
        {
            await DisplayAlert("Error", "Flashlight is not supported on this device.", "OK");
        }
        catch (PermissionException)
        {
            await DisplayAlert("Permission Denied", "Camera permission is required to use the flashlight.", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An unexpected error occurred: {ex.Message}", "OK");
        }
    }

    private async void OnTakePhotoClicked(object sender, EventArgs e)
    {
        try
        {
            if (MediaPicker.Default.IsCaptureSupported)
            {
                var photo = await MediaPicker.Default.CapturePhotoAsync();

                if (photo != null)
                {
                    var stream = await photo.OpenReadAsync();
                    using var memoryStream = new MemoryStream();
                    await stream.CopyToAsync(memoryStream);
                    var imageBytes = memoryStream.ToArray();

                    _capturedImageSource = ImageSource.FromStream(() => new MemoryStream(imageBytes));
                    StatusLabel.Text = "Photo captured successfully.";
                    SemanticScreenReader.Announce("Photo captured successfully.");
                }
            }
            else
            {
                await DisplayAlert("Error", "Camera is not supported on this device.", "OK");
            }
        }
        catch (PermissionException)
        {
            await DisplayAlert("Permission Denied", "Camera permission is required to take photos.", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An unexpected error occurred: {ex.Message}", "OK");
        }
    }

    private async void OnLocateClicked(object sender, EventArgs e)
    {
        try
        {
            StatusLabel.Text = "Fetching location...";

            var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
            var location = await Geolocation.Default.GetLocationAsync(request);

            if (location != null)
            {
                var placemarks = await Geocoding.Default.GetPlacemarksAsync(location);
                var placemark = placemarks?.FirstOrDefault();

                if (placemark != null)
                {
                    _currentLocation = $"{placemark.Locality}, {placemark.AdminArea}, {placemark.CountryName}";
                }
                else
                {
                    _currentLocation = $"Lat: {location.Latitude:F4}, Lon: {location.Longitude:F4}";
                }

                StatusLabel.Text = "Location retrieved successfully.";
                SemanticScreenReader.Announce("Location successfully retrieved.");
            }
            else
            {
                StatusLabel.Text = "Unable to get location.";
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Location error: {ex.Message}", "OK");
            StatusLabel.Text = "Location error.";
        }
    }

    private void OnShareClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(CaloriesEntry.Text))
        {
            DisplayAlert("Validation Error", "Please enter the calories first.", "OK");
            return;
        }

        if (_capturedImageSource == null)
        {
            DisplayAlert("Validation Error", "Please take a photo before sharing.", "OK");
            return;
        }

        // Populate the card
        PostImage.Source = _capturedImageSource;
        PostCaloriesLabel.Text = $"{CaloriesEntry.Text} kcal";
        PostLocationLabel.Text = _currentLocation;
        PostTimeLabel.Text = DateTime.Now.ToString("f");

        PostCard.IsVisible = true;
        StatusLabel.Text = "Shared!";
        SemanticScreenReader.Announce("Post generated successfully.");

        // Ensure the new card inherits the global font scale
        Task.Delay(100).ContinueWith(_ =>
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                DailyFoodSetApp.Services.AccessibilityService.ApplyFontScale(this);
            });
        });
    }
}