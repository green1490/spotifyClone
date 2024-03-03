namespace Routes;

using System.Security.Claims;
using DB;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.HttpResults;
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

        app.MapGet("/logout", (HttpContext ctx) => 
        {
            ctx.SignOutAsync();
        });

        app.MapGet("/login",(HttpContext ctx) =>
        {

            List<Claim> claims =
            [
                new (ClaimTypes.Role, "user"),
            ];

            ClaimsIdentity claimsIdentity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal claimsPrincipal = new(claimsIdentity);

            ctx.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                claimsPrincipal
            );
            return "Successful login!";
        });

        app.MapGet("/auth-test",() =>
        {
            return "Ok";
        }).RequireAuthorization("user_function");
    }
}