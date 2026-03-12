namespace Wordle.Api.Authentication;

internal class ApiKeySettings : AuthenticationSchemeOptions
{
    public string ApiKey { get; set; }
}
