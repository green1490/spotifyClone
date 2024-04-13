namespace Model;

public class Album
{
    public string name { get; set; } = string.Empty;
    public string album_type { get; set; } = string.Empty;
    public string release_date { get; set; } = string.Empty;
    public string release_date_precision { get; set; } = string.Empty;
    public string image_default { get; set; } = string.Empty;
    public string image_large { get; set; } = string.Empty;
}

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

