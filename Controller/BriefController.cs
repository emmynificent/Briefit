using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class BriefController: ControllerBase
{
    private readonly IUrlService _urlService;

    public BriefController(IUrlService urlService)
    {
        _urlService = urlService;
    }

    [HttpPost("shorten")]
    public async Task<IActionResult> ShortenUrl([FromBody] string longUrl)
    {
       var request = new ShortUrlRequest
       {
           OriginalUrl = longUrl
       };
         var response = await _urlService.ShortUrlAsync(request);
        return Ok(new { ShortUrl = response.ShortUrl });
    }

}

