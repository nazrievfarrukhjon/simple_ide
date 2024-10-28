namespace simple_ide;

public class TreeItem
{
    public string Name { get; set; }
    public bool IsFolder { get; set; }
    public List<TreeItem> Children { get; set; } = new();
}