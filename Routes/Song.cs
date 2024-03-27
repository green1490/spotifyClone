using DB;
using System.Text.RegularExpressions;
using RouteInterface;
using Model;

namespace Routes;

public class Song:IRoute
{
    public void Map(WebApplication app)
    {
        app.MapPost("/upload", async (HttpContext context, IFormFile file, DataContext db, string name, string artist) => 
        {
            string pattern = @"^.+\.aac|m4a|mp4|mp3|wav|aac|ogg|flac$";
            Regex regex = new(pattern);
            var extension = Path.GetExtension(file.FileName);
            if (regex.Match(extension).Success)
            {
                using var fileStream = file.OpenReadStream();
                using MemoryStream memStream = new();

                await fileStream.CopyToAsync(memStream);
                var data = memStream.ToArray();
                var userID = context.Session.GetString("userID");
                if(string.IsNullOrEmpty(userID))
                {
                    return "Couldnt upload the file!";
                }
                var song = new Model.Song() 
                { 
                    Name = name,
                    Artist = artist,
                    SongData = data,
                    UserID = Convert.ToInt32(userID)
                };
                await db.AddAsync(song);
                await db.SaveChangesAsync();
                return "Successful upload!";
            }
            return "Wrong file format!";
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
                await db.AddAsync(
                    new History
                    {
                        UserID = Convert.ToInt32(userID),
                        SongID = song.ID
                    });
                await db.SaveChangesAsync();
                ctx.Response.ContentType = "audio/mpeg";
                await ctx.Response.BodyWriter.WriteAsync(song.SongData);
                return "Started to play the music!";
            }
            catch
            {   
                return "Couldnt play the song!";
            }

        }).RequireAuthorization("user_function");
    }

}