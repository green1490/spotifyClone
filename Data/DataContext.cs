using Microsoft.EntityFrameworkCore;
using Model;

namespace DB;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<Users> Users {get;set;}
    public DbSet<Song> Songs {get;set;}
    public DbSet<History> Histories {get;set;}
    public DbSet<Genre> Genres {get;set;}
}