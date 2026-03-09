namespace Wordle.Api.Features.Authentication.Models;

public class ApiKeySettings : AuthenticationSchemeOptions
{
    public string ApiKey { get; set; }
}
