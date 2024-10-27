namespace simple_ide;

using System.Collections.ObjectModel;

public class FileSystemItem
{
    public string Name { get; set; }
    public bool IsDirectory { get; set; }
    public ObservableCollection<FileSystemItem> Children { get; set; }

    public FileSystemItem(string name, bool isDirectory)
    {
        Name = name;
        IsDirectory = isDirectory;
        Children = new ObservableCollection<FileSystemItem>();
    }
}