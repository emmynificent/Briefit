using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("")]
public class RedirectController : ControllerBase
{
    private readonly IUrlService _urlService;

    public RedirectController(IUrlService urlService)
    {
        _urlService = urlService;
    }
    [HttpGet("{shortCode}")]
    public async Task<IActionResult> RedirectToOriginal(string shortCode)
    {
        var originalUrl =  await _urlService.GetOriginalUrlAsync(shortCode);

        if(string.IsNullOrEmpty(originalUrl))
        {
            return NotFound("Short URL not found");
        }
        return Redirect(originalUrl);
    }
}