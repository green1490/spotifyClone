using Model;
using Microsoft.EntityFrameworkCore;

namespace DB;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<Users> Users {get;set;}
    public DbSet<Song> Songs {get;set;}
    public DbSet<Genre> Genres {get;set;}
}