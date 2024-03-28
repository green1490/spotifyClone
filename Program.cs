using DB;
using Routes;
using Microsoft.EntityFrameworkCore;
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

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromDays(1);
    options.Cookie.IsEssential = true;
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
app.UseSession();
app.MapControllers();

var handler = new Routes.RouteHandler(app);
var singleton = RouteSingleton.GetInstance();

handler
    .Add(singleton.user)
    .Add(singleton.song);

handler.Map();
app.Run();
