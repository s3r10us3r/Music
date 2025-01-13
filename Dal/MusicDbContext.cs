using Microsoft.EntityFrameworkCore;
using Models;

namespace Dal;

public sealed class MusicDbContext : DbContext
{
    public DbSet<Album> Albums { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<User> Users { get; set; }

    public MusicDbContext()
    {
        Database.EnsureCreated();
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseLazyLoadingProxies()
            .UseSqlite("Data Source=music.db");
    }
}