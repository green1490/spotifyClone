namespace Model;

public class Item
{
    public string artist { get; set; } = string.Empty;
    public string id { get; set; } = string.Empty;
    public string image { get; set; } = string.Empty;
    public string name { get; set; } = string.Empty;
}

public class ApiSearch
{
    public Tracks tracks { get; set; } = new();
}

public class Tracks
{
    public List<Item> items { get; set; } = new();
}