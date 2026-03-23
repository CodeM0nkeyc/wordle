namespace Wordle.Api.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = ApiKeyAuthenticationDefaults.Scheme)]
public class WordleController : ControllerBase
{
    private readonly ILogger<WordleController> _logger;
    private readonly IWordleStorage _wordleStorage;

    public WordleController(IWordleStorage wordleStorage, ILogger<WordleController> logger)
    {
        _wordleStorage = wordleStorage;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<string>> GetWordAsync()
    {
        try
        {
            WordleEntity word = (await _wordleStorage.GetWordAsync())!;

            if (Request.Headers.TryGetValue("X-TimeZone", out var timeZoneOffset))
            {
                TimeSpan offset = TimeSpan.Parse(timeZoneOffset.ToString());
                DateTime startUtc = WordleDefaults.StartRefreshUtc.Subtract(TimeSpan.FromDays(1));

                if (DateTime.UtcNow + offset < startUtc)
                {
                    return Ok(word.OldWord);
                }
            }

            return Ok(word.CurrentWord);
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Error occured while requesting random word.");
            return StatusCode(500);
        }
    }
}
