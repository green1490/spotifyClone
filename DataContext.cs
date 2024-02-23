using Microsoft.EntityFrameworkCore;
using Model;

namespace DB;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<Users> Users {get;set;}
}