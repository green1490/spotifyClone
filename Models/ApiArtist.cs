// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Artist
    {
        public string id { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public string popularity { get; set; } = string.Empty;
        public string followers { get; set; } = string.Empty;
        public string image { get; set; } = string.Empty;
        public string updated_date { get; set; } = string.Empty;
        public List<string> genres { get; set; } = [];
        public int cached { get; set; }
    }

    public class ApiArtist
    {
        public List<Artist> artists { get; set; } = [];
    }

