public interface IUrlService
{
    Task<ShortUrlResponse> ShortUrlAsync (ShortUrlRequest originalUrl);
    Task<string> GetOriginalUrlAsync (string shortUrl);     
}