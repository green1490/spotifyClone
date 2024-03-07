namespace Routes;

using DB;
using Model;
using RouteInterface;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Antiforgery;

public class User:IRoute
{
    public void Map(WebApplication app)
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

        app.MapGet("/logout", (HttpContext ctx) => 
        {
            ctx.SignOutAsync();
        });

        app.MapGet("/login",async (HttpContext ctx, DataContext db, string name, string password) =>
        {   
            try
            {
                var result = await db.Users.Where(field => field.Name == name && field.Password == password).FirstAsync();
            }
            catch
            {
                return "Unseccessful login!";
            }

            List<Claim> claims =
            [
                new (ClaimTypes.Role, "user"),
            ];

            ClaimsIdentity claimsIdentity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal claimsPrincipal = new(claimsIdentity);

            await ctx.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                claimsPrincipal
            );
            return "Successful login!";
        });

        app.MapPut("/password", async (Users user, DataContext db) => 
        {
            var result = await db.Users.Where(field => field.Name == user.Name).FirstAsync();
            result.Password = user.Password;
            await db.SaveChangesAsync();
            return "Changed Password!";
        }).RequireAuthorization("user_function");

        app.MapGet("/AF-token",(HttpContext context,IAntiforgery antiforgery) =>
        {
            var tokenSet = antiforgery.GetAndStoreTokens(context);
            context.Response.Cookies.Append("XSRF-TOKEN", tokenSet.RequestToken!);;
        });
    }
}