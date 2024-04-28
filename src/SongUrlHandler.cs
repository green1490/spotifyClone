using Model;
using StringResources;
using System.Text.Json;

namespace url;

public class SongUrlHandler
{
    private string SongID {get;set;} = string.Empty;
    private string Name {get;set;} = string.Empty;
    private string Artist {get;set;} = string.Empty;
    private HttpClient Client {get;} = new();
    private SongUrlHandler() {}

    /// <param name="name"></param>
    /// <param name="artist"></param>
    /// <exception cref="ArgumentException">Null or empty is the argument</exception>
    /// <exception cref="InvalidOperationException">Couldnt find the song ID</exception>
    public static async Task<SongUrlHandler> CreateAsync(string name, string artist)
    {
        if(string.IsNullOrEmpty(name) || string.IsNullOrEmpty(name))
        {
            throw new ArgumentException();
        }

        SongUrlHandler handler = new()
        {
            Name = name.Trim().ToLower(),
            Artist = artist.Trim().ToLower()
        };
        handler.Client.DefaultRequestHeaders.Add("Accept","application/json");
        handler.Client.DefaultRequestHeaders.Add("Referer","https://www.chosic.com/music-genre-finder");
        var id = await handler.GetSongID();

        if(string.IsNullOrEmpty(id))
        {
            throw new InvalidOperationException(StringSingleton.InformationError);
        }
        else
        {
            handler.SongID = id;
        }

        return handler;
    }

    private async Task<string?> GetSongID()
    {
        var tracks = await Client.GetAsync($"https://www.chosic.com/api/tools/search?q={Name}&type=track&limit=10");
        var response = await tracks.Content.ReadAsStreamAsync();
        var idApiResponse = await JsonSerializer.DeserializeAsync<ApiSearch>(response);
        foreach(var item in idApiResponse?.tracks.items ?? [])
        {
            if(string.Equals(Artist,item.artist.Trim().ToLower()) && string.Equals(Name, item.name.Trim().ToLower()))
            {
                return item.id;
            }
        }
        return null;
    }

    /// <exception cref="InvalidOperationException">Couldnt find the song ID</exception>
    public async Task<List<string>> ListSongGenres()
    {
        var httpMessage = await Client.GetAsync($"https://www.chosic.com/api/tools/tracks/{SongID}");
        var response = await httpMessage.Content.ReadAsStreamAsync();
        var trackApiResponse = await JsonSerializer.DeserializeAsync<ApiTracks>(response);
        var artist = trackApiResponse?.artists.First();

        if (artist is not null)
        {
            httpMessage = await Client.GetAsync($"https://www.chosic.com/api/tools/artists?ids={artist.id}");
            response = await httpMessage.Content.ReadAsStreamAsync();
            var artistApiResponse = await JsonSerializer.DeserializeAsync<ApiArtist>(response);
            var genres = (artistApiResponse?.artists.First().genres) ?? throw new InvalidOperationException(StringSingleton.GenreError);
            return genres;
        }
        throw new InvalidOperationException(StringSingleton.ArtistError);
    }

    ~SongUrlHandler()
    {
        Client.Dispose();
    }
}