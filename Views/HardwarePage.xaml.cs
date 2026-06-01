using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Devices.Sensors;
using Microsoft.Maui.Media;
using Microsoft.Maui.Graphics;

namespace DailyFoodSetApp.Views;

public class SocialPost
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public string ImagePath { get; set; }
    public string Description { get; set; }
    public string LocationInfo { get; set; }

    public bool HasImage => !string.IsNullOrEmpty(ImagePath);
    public bool HasLocation => !string.IsNullOrWhiteSpace(LocationInfo);
}

public partial class HardwarePage : ContentPage
{
    public ObservableCollection<SocialPost> FeedPosts { get; set; } = new();

    private bool _isFlashlightOn = false;
    private byte[] _currentImageBytes;
    private string _currentLocationText;

    public HardwarePage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    private void TriggerHapticFeedback()
    {
        try
        {
            HapticFeedback.Default.Perform(HapticFeedbackType.Click);
        }
        catch { }
    }

    private async void OnTakePhotoClicked(object sender, EventArgs e)
    {
        TriggerHapticFeedback();
        try
        {
            if (_isFlashlightOn)
            {
                await Flashlight.Default.TurnOffAsync();
                _isFlashlightOn = false;
                FlashlightButton.Text = "Turn On Flashlight";
                FlashlightButton.BackgroundColor = Color.FromArgb("#512BD4");

                await Task.Delay(300);
            }

            if (!MediaPicker.Default.IsCaptureSupported)
            {
                await DisplayAlert("Oops!", "It looks like your device doesn't support the camera.", "Okay");
                return;
            }

            var photoResult = await MediaPicker.Default.CapturePhotoAsync();
            if (photoResult != null)
            {
                await using var stream = await photoResult.OpenReadAsync();
                using var memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);

                _currentImageBytes = memoryStream.ToArray();
                FoodPhoto.Source = ImageSource.FromStream(() => new MemoryStream(_currentImageBytes));
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Oops!", ex.Message, "Okay");
        }
    }

    private async void OnToggleFlashlightClicked(object sender, EventArgs e)
    {
        TriggerHapticFeedback();
        try
        {
            if (_isFlashlightOn)
            {
                await Flashlight.Default.TurnOffAsync();
                _isFlashlightOn = false;
                FlashlightButton.Text = "Turn On Flashlight";
                FlashlightButton.BackgroundColor = Color.FromArgb("#512BD4");
            }
            else
            {
                await Flashlight.Default.TurnOnAsync();
                _isFlashlightOn = true;
                FlashlightButton.Text = "Turn Off Flashlight";
                FlashlightButton.BackgroundColor = Color.FromArgb("#A95517");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Oops!", $"Flashlight issue: {ex.Message}", "Okay");
        }
    }

    private async void OnGetLocationClicked(object sender, EventArgs e)
    {
        TriggerHapticFeedback();
        try
        {
            var parameters = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
            var locationResult = await Geolocation.Default.GetLocationAsync(parameters);

            if (locationResult != null)
            {
                CoordinateLabel.Text = $"Lat: {locationResult.Latitude:F5}, Lon: {locationResult.Longitude:F5}";

                var placemarks = await Geocoding.Default.GetPlacemarksAsync(locationResult);
                var mark = placemarks?.FirstOrDefault();

                if (mark != null)
                {
                    _currentLocationText = $"{mark.Locality}, {mark.AdminArea}, {mark.CountryName}";
                }
                else
                {
                    _currentLocationText = $"Lat {locationResult.Latitude:F2}, Lon {locationResult.Longitude:F2}";
                }

                LocationLabel.Text = _currentLocationText;
                await DisplayAlert("Got it!", "Your location has been added to your post.", "Awesome");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Oops!", "We couldn't get your location. Please check your device settings.", "Okay");
        }
    }

    private void OnShareClicked(object sender, EventArgs e)
    {
        TriggerHapticFeedback();
        string textContent = PostDescriptionEditor.Text?.Trim();

        if (string.IsNullOrWhiteSpace(textContent) && _currentImageBytes == null)
        {
            DisplayAlert("Almost there!", "Please add a photo or write something before sharing.", "Okay");
            return;
        }

        string savedImagePath = null;
        if (_currentImageBytes != null)
        {
            savedImagePath = Path.Combine(FileSystem.CacheDirectory, $"post_{Guid.NewGuid():N}.jpg");
            File.WriteAllBytes(savedImagePath, _currentImageBytes);
        }

        var newPost = new SocialPost
        {
            Description = string.IsNullOrWhiteSpace(textContent) ? "Enjoying a great meal!" : textContent,
            LocationInfo = _currentLocationText,
            ImagePath = savedImagePath
        };

        FeedPosts.Insert(0, newPost);

        PostDescriptionEditor.Text = string.Empty;
        FoodPhoto.Source = null;
        _currentImageBytes = null;
        CoordinateLabel.Text = "Location not added yet";
        LocationLabel.Text = "Tap the button above to add your location.";
        _currentLocationText = null;
    }

    private async void OnDeletePostClicked(object sender, EventArgs e)
    {
        TriggerHapticFeedback();
        if (sender is Button triggerButton && triggerButton.CommandParameter is string targetId)
        {
            bool confirm = await DisplayAlert("Remove Post?", "Are you sure you want to delete this post? It will be gone forever.", "Yes, remove it", "No, keep it");
            if (!confirm) return;

            var postToRemove = FeedPosts.FirstOrDefault(p => p.Id == targetId);
            if (postToRemove != null)
            {
                if (!string.IsNullOrEmpty(postToRemove.ImagePath) && File.Exists(postToRemove.ImagePath))
                {
                    try { File.Delete(postToRemove.ImagePath); } catch { }
                }
                FeedPosts.Remove(postToRemove);
            }
        }
    }
}