using Wordle.Api.Misc.Settings;

namespace Wordle.Api.Misc.JwtKeyAccessor;

// NOT USED
public interface IJwtKeyAccessor
{
    public byte[] Key { get; }
    public Task SetNewKeyPasswordAsync(IAppSettingsService appSettingsService, string password);
}
