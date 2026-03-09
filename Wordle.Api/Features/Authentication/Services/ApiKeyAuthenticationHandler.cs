namespace Wordle.Api.Features.Authentication.Services;

public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeySettings>
{
    public ApiKeyAuthenticationHandler(
        IOptionsMonitor<ApiKeySettings> opts,
        ILoggerFactory loggerFactory,
        UrlEncoder urlEncoder) : base(opts, loggerFactory, urlEncoder)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        string[] apiKeyValues = Context.Request.Headers.Authorization.ToString().Split(' ');

        if (apiKeyValues.Length < 2)
        {
            return Task.FromResult(AuthenticateResult.Fail("Authentication parameters are missed in request."));
        }

        (string? authScheme, string? apiKey) = (apiKeyValues[0], apiKeyValues[1]);

        if (authScheme != Scheme.Name || apiKey is null)
        {
            return Task.FromResult(AuthenticateResult.Fail("Invalid authentication parameters in request."));
        }

        if (apiKey == Options.ApiKey)
        {
            AuthenticationTicket ticket = BuildAuthenticationTicket();

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }

        return Task.FromResult(AuthenticateResult.Fail("Invalid api key in request."));
    }

    private AuthenticationTicket BuildAuthenticationTicket()
    {
        ClaimsIdentity identity = new ClaimsIdentity(Scheme.Name);
        ClaimsPrincipal principal = new ClaimsPrincipal(identity);

        AuthenticationTicket ticket = new AuthenticationTicket(principal, Scheme.Name);

        return ticket;
    }
}
