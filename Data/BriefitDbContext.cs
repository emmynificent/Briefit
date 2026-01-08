using Microsoft.EntityFrameworkCore;

public class BriefitDbContext : DbContext
{
    public BriefitDbContext(DbContextOptions<BriefitDbContext> options) : base(options)
    {
    }

    public DbSet<ShortUrl> ShortUrls { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder){
        modelBuilder.Entity<ShortUrl>()
        .HasIndex(u => u.ShortCode)
        .IsUnique();

        modelBuilder.Entity<ShortUrl>()
        .HasIndex(u => u.OriginalUrl);
    }
}