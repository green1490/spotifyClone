using DB;
using url;
using Model;
using RouteInterface;
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
                return "Wrong file format!";
            }

            using var fileStream = file.OpenReadStream();
            using MemoryStream memStream = new();

            await fileStream.CopyToAsync(memStream);
            var data = memStream.ToArray();
            var userID = context.Session.GetString("userID");
            if(string.IsNullOrEmpty(userID))
            {
                return "Sign in to upload!";
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
                return "Successful upload!";
            }
            catch
            {
                return "Unsuccessful upload!";
            }
        })
            .RequireAuthorization("user_function")
            .DisableAntiforgery();
        app.MapGet("/play",async (HttpContext context ,DataContext db, HttpContext ctx, string name, string artist) =>
        {
            try
            {
                var song = db.Songs.Where(row => row.Name == name && row.Artist == artist).First();
                var userID = context.Session.GetString("userID");
                if(string.IsNullOrEmpty(userID))
                {
                    return "Couldnt get the user!";
                }
                ctx.Response.ContentType = "audio/mpeg";
                await ctx.Response.BodyWriter.WriteAsync(song.SongData);
                await db.AddAsync(
                    new History
                    {
                        UserID = Convert.ToInt32(userID),
                        SongID = song.ID
                    });
                await db.SaveChangesAsync();
                return "Started to play the music!";
            }
            catch
            {   
                return "Couldnt play the song!";
            }
        }).RequireAuthorization("user_function");
    }
}