namespace Routes;

#nullable disable
public class RouteSingleton
{
    private RouteSingleton() {}
    private static RouteSingleton _singleton;

    public User user = new();
    public Song song = new();
    public Playlist playlist = new();
    public PlaylistContent playlistManager = new();

    public static RouteSingleton GetInstance()
    {
        if (_singleton is null)
        {
            _singleton = new RouteSingleton();
        }
        return _singleton;
    }
}