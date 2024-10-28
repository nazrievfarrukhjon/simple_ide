using System.Xml.Linq;

namespace simple_ide;

using Microsoft.Maui.Storage;
using Microsoft.Maui.Controls;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        
        TreeData = new List<TreeItem>
        {
            new TreeItem
            {
                Name = "Root Folder",
                IsFolder = true,
                Children = new List<TreeItem>
                {
                    new TreeItem
                    {
                        Name = "Subfolder 1",
                        IsFolder = true,
                        Children = new List<TreeItem>
                        {
                            new TreeItem { Name = "File 1.txt", IsFolder = false },
                            new TreeItem { Name = "File 2.txt", IsFolder = false }
                        }
                    },
                    new TreeItem
                    {
                        Name = "Subfolder 2",
                        IsFolder = true,
                        Children = new List<TreeItem>
                        {
                            new TreeItem { Name = "File 3.txt", IsFolder = false }
                        }
                    },
                    new TreeItem { Name = "File 4.txt", IsFolder = false }
                }
            }
        };

        // Set the data context
        BindingContext = this;
    }

    private void OnFileClicked(object sender, EventArgs e)
    {
        DisplayAlert("File", "File operations will go here.", "OK");
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

            if (result != null)
            {
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
        catch (Exception ex)
        {
            Console.WriteLine($"Error picking file: {ex.Message}");
        }
    }

    private async void OnSaveFileButtonClicked(object sender, EventArgs e)
    {
        try
        {
            // Ask the user for a directory where they want to save the file
            string directoryPath = await DisplayPromptAsync("Directory Path",
                "Enter the directory path to save your file:", "OK", "Cancel", "/Users/farruqnazri/Desktop");

            // Ask the user for a file name
            string fileName = await DisplayPromptAsync("File Name", "Enter the name of the file (with .txt extension):",
                "OK", "Cancel", "filename.txt");

            // Ensure a valid directory path and file name is provided
            if (!string.IsNullOrWhiteSpace(directoryPath) &&
                !string.IsNullOrWhiteSpace(fileName) &&
                fileName.EndsWith(".txt"))
            {
                // Construct the full path for saving the file
                var filePath = System.IO.Path.Combine(directoryPath.Trim(), fileName);

                // Create the directory if it doesn't exist
                if (!System.IO.Directory.Exists(directoryPath))
                {
                    System.IO.Directory.CreateDirectory(directoryPath);
                }

                // Attempt to write the text from the TextEditor to the specified file
                try
                {
                    await System.IO.File.WriteAllTextAsync(filePath, TextEditor.Text);
                    await DisplayAlert("Success", $"File saved to: {filePath}", "OK");
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"Could not save the file: {ex.Message}", "OK");
                }
            }
            else
            {
                await DisplayAlert("Error", "Please enter a valid directory path and file name with a .txt extension.",
                    "OK");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving file: {ex.Message}");
        }
    }
    
    private bool _isFolderPanelVisible = true;
    
    private void ToggleFolders_Clicked(object sender, EventArgs e)
    {
        _isFolderPanelVisible = !_isFolderPanelVisible;
    
        // Set the visibility of the folder panel and separators
        FolderPanel.IsVisible = _isFolderPanelVisible;
        LeftSeparator.IsVisible = _isFolderPanelVisible;
        RightSeparator.IsVisible = _isFolderPanelVisible;

        if (_isFolderPanelVisible)
        {
            // Set FolderPanel width to 200 and Editor column span to 1
            FolderPanel.WidthRequest = 200;
            Grid.SetColumnSpan(TextEditor, 1);
        }
        else
        {
            // Set FolderPanel width to 0 and make Editor span two columns
            FolderPanel.WidthRequest = 0;
            Grid.SetColumnSpan(TextEditor, 2);
        }
    }
    
    public List<TreeItem> TreeData { get; set; }


}