using Model;
using System.Text.Json;

namespace url;

public class UrlHandler
{
    private string SongID {get;set;} = string.Empty;
    private string Name {get;set;} = string.Empty;
    private string Artist {get;set;} = string.Empty;
    private UrlHandler() {}

    /// <param name="name"></param>
    /// <param name="artist"></param>
    /// <exception cref="ArgumentException">Null or empty is the argument</exception>
    /// <exception cref="InvalidOperationException">Couldnt find the song ID</exception>
    public static async Task<UrlHandler> CreateAsync(string name, string artist)
    {
        if(string.IsNullOrEmpty(name) || string.IsNullOrEmpty(name))
        {
            throw new ArgumentException();
        }

        UrlHandler handler = new()
        {
            Name = name.Trim().ToLower(),
            Artist = artist.Trim().ToLower()
        };
        var id = await handler.GetSongID();
        if(string.IsNullOrEmpty(id))
        {
            throw new InvalidOperationException();
        }
        else
        {
            handler.SongID = id;
        }

        return handler;
    }

    private async Task<string?> GetSongID()
    {
        using HttpClient client = new();
        client.DefaultRequestHeaders.Add("Accept","application/json");
        client.DefaultRequestHeaders.Add("Referer","https://www.chosic.com/music-genre-finder");
        var tracks = await client.GetAsync($"https://www.chosic.com/api/tools/search?q={Name}&type=track&limit=10");
        var response = await tracks.Content.ReadAsStreamAsync();
        var idApiResponse = await JsonSerializer.DeserializeAsync<ApiID>(response);
        foreach(var item in idApiResponse?.tracks.items ?? [])
        {
            if(string.Equals(Artist,item.artist.Trim().ToLower()) && string.Equals(Name, item.name.Trim().ToLower()))
            {
                return item.id;
            }
        }
        return null;
    }
}