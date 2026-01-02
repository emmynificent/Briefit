public class ShortUrlResponse
{   
    public int Id {get; set; }
    public string ShortUrl {get; set; }
    public string OriginalUrl {get; set;}
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}