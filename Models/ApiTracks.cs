namespace Model;

public class Artist
{
    public string id { get; set; } = string.Empty;
    public string name { get; set; } = string.Empty;
}

public class ApiTracks
{
    public string id { get; set; } = string.Empty;
    public string name { get; set; } = string.Empty;
    public List<Artist> artists { get; set; } = [];
    public string preview_url { get; set; } = string.Empty;
    public string duration_ms { get; set; } = string.Empty;
    public string popularity { get; set; } = string.Empty;
    public Album album { get; set; } = new();
    public string updated_date { get; set; } = string.Empty;
    public int cached { get; set; }
}

