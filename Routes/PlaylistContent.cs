using DB;
using RouteInterface;
using StringResources;
using Microsoft.EntityFrameworkCore;

namespace Routes;

public class PlaylistContent : IRoute
{
    public void Map(WebApplication app)
    {   
        app.MapPost("/playlist/{name}", async (string name, string songName, string artist, DataContext db) =>
        {
            try
            {
                var playlist = db.Playlists.Where(playlist => playlist.Name == name).First();
                var song = db.Songs.Where(song => song.Name == songName && song.Artist == artist).First();
                var numberOfItemInPlaylist = db.PlaylistContents.Where(item => item.PlaylistID == playlist.ID && item.SongID == song.ID).Count();
                if (numberOfItemInPlaylist > 0) 
                {
                    return StringSingleton.PlaylistAlreadyAdded;
                }

                var newAddedSong = new Model.PlaylistContent
                {
                    PlaylistID = playlist.ID,
                    SongID = song.ID
                };

                await db.PlaylistContents.AddAsync(newAddedSong);
                await db.Database.ExecuteSqlAsync($"CALL update_modification({playlist.ID})");
                await db.SaveChangesAsync();

                return StringSingleton.PlaylistAddedSong;
            }
            catch
            {
                return StringSingleton.PlaylistAddedSongError;
            }
        }).RequireAuthorization("user_function");

        app.MapDelete("/playlist/{name}", (string name, string songName, string artist, DataContext db) => 
        {   
            try
            {
                var song = db.Songs.Where(song => song.Name == songName && song.Artist == artist).First();
                var playlist = db.Playlists.Where(playlist => playlist.Name == name).First();

                var removedSong = db.PlaylistContents.Where(item => item.PlaylistID == playlist.ID && item.SongID == song.ID).First();
                db.PlaylistContents.Remove(removedSong);
                db.SaveChanges();

                return StringSingleton.PlaylistRemoveSong;
            }
            catch
            {
                return StringSingleton.PlaylistRemoveSongError;
            }

        }).RequireAuthorization("user_function");

        app.MapGet("/playlist/{name}", (string name, DataContext db) =>
        {   
            var songsInPlaylist =   from playlist in db.Playlists
                                    join content in db.PlaylistContents on playlist.ID equals content.PlaylistID
                                    join song in db.Songs on content.SongID equals song.ID
                                    where playlist.Name == name
                                    select new{song.Artist,song.Name};
            return songsInPlaylist;
        }).RequireAuthorization("user_function");
    }
}