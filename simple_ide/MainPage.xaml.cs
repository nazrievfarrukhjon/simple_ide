namespace simple_ide;

using Microsoft.Maui.Storage;
using Microsoft.Maui.Controls;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private void OnFileClicked(object sender, EventArgs e)
    {
        // Handle file operations (e.g., new, open, save)
        DisplayAlert("File", "File operations will go here.", "OK");
    }

    private void OnToolsClicked(object sender, EventArgs e)
    {
        // Handle tool operations (e.g., formatting, options)
        DisplayAlert("Tools", "Tool operations will go here.", "OK");
    }

    private void OnHelpClicked(object sender, EventArgs e)
    {
        // Show help information
        DisplayAlert("Help", "Help information will go here.", "OK");
    }

    // Optional: If you want to keep the counter functionality, 
    // you can implement it as needed.
    private int count = 0;

    private void OnCounterClicked(object sender, EventArgs e)
    {
        count++;

        // Example of how you could use the counter, but this can be removed 
        // if you only want the text editor features.
        DisplayAlert("Counter", $"Clicked {count} time{(count > 1 ? "s" : "")}.", "OK");
    }

    private void TapGestureRecognizer_OnTapped(object sender, TappedEventArgs e)
    {
        TextEditor.Focus();
    }

    private async void OnOpenFileButtonClicked(object sender, EventArgs e)
    {
        try
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Please select a file"
            });

            if (result != null) {
                var filePickerFileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.Android, new[] { "text/plain", ".txt" } },
                    { DevicePlatform.iOS, new[] { "text/plain", ".txt" } },
                    { DevicePlatform.WinUI, new[] { "text/plain", ".txt" } }
                });

                var filePath = result.FullPath;
                try
                {
                    var content = await File.ReadAllTextAsync(filePath);
                    TextEditor.Text = content;
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"Could not read the file: {ex.Message}", "OK");
                }
            }
        }
        catch (Exception ex) {
            Console.WriteLine($"Error picking file: {ex.Message}");
        }
    }
}