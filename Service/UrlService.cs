using Microsoft.EntityFrameworkCore;
//using briefit.DTO;

public class UrlService : IUrlService
{
    private readonly BriefitDbContext _briefitDbContext;
    private readonly IConfiguration _configuration;

    public UrlService(BriefitDbContext briefitDbContext, IConfiguration configuration)
    {
        _briefitDbContext = briefitDbContext;
        _configuration = configuration;        
    }

    public async Task<ShortUrlResponse> ShortUrlAsync(ShortUrlRequest request)
    {

        var sw = System.Diagnostics.Stopwatch.StartNew();

        var exisitingUrl = await _briefitDbContext.ShortUrls.FirstOrDefaultAsync(u => u.OriginalUrl == request.OriginalUrl);

        Console.WriteLine($"Duplicate check took: {sw.ElapsedMilliseconds} ms");


        if(exisitingUrl != null)
        {
            var baseUrl = GetBaseUrl();
            return new ShortUrlResponse
            {
                ShortUrl = $"{baseUrl}/{exisitingUrl.ShortCode}",
                OriginalUrl = exisitingUrl.OriginalUrl,
                CreatedAt = exisitingUrl.CreatedAt,
            };
        }

        sw.Restart();


        var shortCode= ShortCodeGenerator.Generate();

        while (await _briefitDbContext.ShortUrls.AnyAsync(u => u.ShortCode == shortCode))
        {
            shortCode = ShortCodeGenerator.Generate();
        }

        Console.WriteLine($"Short code generation took: {sw.ElapsedMilliseconds} ms");

        sw.Restart();

        var shortUrlEntity = new ShortUrl   
        {
            OriginalUrl = request.OriginalUrl,
            ShortCode = shortCode,
            CreatedAt = DateTime.UtcNow,
            ClickCount = 0
        };

        _briefitDbContext.ShortUrls.Add(shortUrlEntity);

        await _briefitDbContext.SaveChangesAsync();

        var baseURL = GetBaseUrl();
        
        return new ShortUrlResponse
        {
            ShortUrl = $"{baseURL}/{shortUrlEntity.ShortCode}",
            OriginalUrl = shortUrlEntity.OriginalUrl,
            CreatedAt = shortUrlEntity.CreatedAt
        };

    }

    public async Task<string> GetOriginalUrlAsync(string shortCode)
    {
        var urlInstance = await _briefitDbContext.ShortUrls.FirstOrDefaultAsync(u => u.ShortCode == shortCode);
        
        if(urlInstance == null)
        {
            return null;
        }    

        urlInstance.ClickCount ++ ;
        urlInstance.LastClickedAt = DateTime.UtcNow;
        await _briefitDbContext.SaveChangesAsync();

        return urlInstance.OriginalUrl;
    
    }

    private string GetBaseUrl()
    {
        return _configuration["BaseUrl"] ?? "http://localhost:5228";
    }
}