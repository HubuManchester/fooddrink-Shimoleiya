using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Maui.Media;
using Microsoft.Maui.Controls;

namespace DailyFoodSetApp.Views;

public partial class HardwarePage : ContentPage
{
    public HardwarePage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        DailyFoodSetApp.Services.AccessibilityService.ApplyFontScale(this);
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

                    CapturedImage.Source = ImageSource.FromStream(() => new MemoryStream(imageBytes));

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
}