using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;

public class BriefitDbContextFactory : IDesignTimeDbContextFactory<BriefitDbContext>
{
    public BriefitDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<BriefitDbContext>();

        var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

        if (!string.IsNullOrEmpty(databaseUrl))
        {
            Console.WriteLine($"Using PostgreSQL. URL starts with: {databaseUrl.Substring(0, Math.Min(30, databaseUrl.Length))}...");

            var connectionString = ConvertDatabaseUrl(databaseUrl);
            Console.WriteLine($"Converted to: {connectionString.Substring(0, Math.Min(50, connectionString.Length))}...");

            optionsBuilder.UseNpgsql(connectionString);
        }
        else
        {
            Console.WriteLine("Using SQL Server (local)");
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=BriefitDb;Trusted_Connection=True;MultipleActiveResultSets=true");
        }

        return new BriefitDbContext(optionsBuilder.Options);
    }

    private string ConvertDatabaseUrl(string databaseUrl)
    {
        try
        {
            // Handle both postgres:// and postgresql://
            var uri = new Uri(databaseUrl.Replace("postgres://", "postgresql://"));
            var userInfo = uri.UserInfo.Split(':');

            // Default port is 5432 for PostgreSQL
            var port = uri.Port > 0 ? uri.Port : 5432;
            var database = uri.AbsolutePath.TrimStart('/');

            var connectionString = $"Host={uri.Host};Port={port};Database={database};Username={userInfo[0]};Password={userInfo[1]};SSL Mode=Require;Trust Server Certificate=true";

            return connectionString;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error parsing DATABASE_URL: {ex.Message}");
            throw;
        }
    }
}