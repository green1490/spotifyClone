namespace Routes;

using DB;
using Model;

public static class User
{
    public static void Map(WebApplication app)
    {
        app.MapPost("/registration", async (Users user, DataContext db) =>
        {
            try
            {
                await db.AddAsync(user);
                await db.SaveChangesAsync();
                return "Successful registration!";
            }
            catch
            {
                return "Unseccessful registration!";
            }
        });
    }
}