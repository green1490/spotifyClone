using DB;
using url;
using RouteInterface;
using StringResources;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Routes;

public class Song:IRoute
{
    public void Map(WebApplication app)
    {
        //uploads the song and inserts data into Genre and GenreSong
        app.MapPost("/upload", async (HttpContext context, IFormFile file, DataContext db, string name, string artist) => 
        {
            string pattern = @"^.+\.aac|m4a|mp4|mp3|wav|aac|ogg|flac$";
            Regex regex = new(pattern);
            var extension = Path.GetExtension(file.FileName);
            if (regex.Match(extension).Success == false)
            {
                return StringSingleton.WrongFormat;
            }

            using var fileStream = file.OpenReadStream();
            using MemoryStream memStream = new();

            await fileStream.CopyToAsync(memStream);
            var data = memStream.ToArray();
            var userID = context.Session.GetString("userID");
            if(string.IsNullOrEmpty(userID))
            {
                return StringSingleton.SignIn;
            }
            
            var song = new Model.Song() 
            { 
                Name = name,
                Artist = artist,
                SongData = data,
                UserID = Convert.ToInt32(userID)
            };
            try
            {
                // Wrapper around the site's API
                var handler = await SongUrlHandler.CreateAsync(name, artist);
                var genres = await handler.ListSongGenres();

                await db.AddAsync(song);
                await db.SaveChangesAsync();
                await db.Database.ExecuteSqlAsync($"CALL insert_genre ({song.ID},{genres})");
                return StringSingleton.SuccessfulUpload;
            }
            catch
            {
                return StringSingleton.UnsuccessfulUpload;
            }
        })
            .RequireAuthorization("user_function")
            .DisableAntiforgery();

    app.MapGet("/play", async (DataContext db, string name, string artist, HttpContext ctx) =>
    {
        var song = db.Songs.Where(row => row.Name == name && row.Artist == artist).First();
        var userID = ctx.Session.GetString("userID");
        if (string.IsNullOrEmpty(userID) == false)
        {
            ctx.Response.ContentType = "audio/mpeg";
            await ctx.Response.BodyWriter.WriteAsync(song.SongData);
        }
    }).RequireAuthorization("user_function");

    app.MapGet("/recommendation",async (string name, string artist, string limit) =>
    {
        var handler = await SongUrlHandler.CreateAsync(name, artist);
        return await handler.RecommendSongs(limit);

    }).RequireAuthorization("user_function");
    }
}