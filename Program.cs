using DB;
using Routes;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => 
    {
        options.ExpireTimeSpan = TimeSpan.FromDays(3);
    });

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("user_function", policy => 
        policy.RequireRole("user"));

builder.Services.AddSwaggerGen(
    c =>
    {
        c.EnableAnnotations();
    }
);
builder.Services.AddDbContext<DataContext>(option => 
{
    option.UseNpgsql(builder.Configuration.GetConnectionString("DataBase"));
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

User.Map(app);

app.Run();
