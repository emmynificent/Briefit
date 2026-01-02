using Microsoft.EntityFrameworkCore;

public class BriefitDbContext : DbContext
{
    public BriefitDbContext(DbContextOptions<BriefitDbContext> options) : base(options)
    {
    }

    public DbSet<ShortUrl> ShortUrls { get; set; }
}