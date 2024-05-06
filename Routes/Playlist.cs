using DB;
using RouteInterface;
using StringResources;

namespace Routes;

public class Playlist : IRoute
{
    public void Map(WebApplication app)
    {   
        app.MapPost("/playlist",async (Model.Playlist playlist, DataContext db, HttpContext ctx) =>
        {
            var userID = ctx.Session.GetString("userID");
            if (string.IsNullOrEmpty(userID))
            {
                return StringSingleton.SignIn;
            }
            playlist.UserID = Convert.ToInt32(userID);
            await db.Playlists.AddAsync(playlist);
            await db.SaveChangesAsync();
            return StringSingleton.Playlist;
        }).RequireAuthorization("user_function");
        
        app.MapGet("/playlist", (DataContext db, string name) =>
        {
            return db.Playlists.Where(item => item.Name == name);
        }).RequireAuthorization("user_function");

        app.MapDelete("/playlist", (string name, DataContext db) =>
        {
            var playlist = db.Playlists.Where(item => item.Name == name).First();
            db.Playlists.Remove(playlist);
        }).RequireAuthorization("user_function");
    }
}