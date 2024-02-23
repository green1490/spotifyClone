namespace Routes;

using DB;
using Model;

public static class User
{
    public static void Map(WebApplication app)
    {
        app.MapPost("/registration", (Users user, DataContext db) =>
        {
            
        });
    }
}