using Wordle.Api.Misc.Settings;

namespace Wordle.Api.Misc.JwtKeyAccessor;

// NOT USED
public class JwtKeyAccessor : IJwtKeyAccessor
{
    private const int _iterationsCount = 600000;
    private readonly static HashAlgorithmName _hashName = HashAlgorithmName.SHA3_512;

    public byte[] Key { get; private set; }

    private JwtKeyAccessor()
    {
    }

    public static JwtKeyAccessor Create(
        IConfiguration configuration,
        string password)
    {
        var keySection = configuration.GetSection("JwtSigningKey")!;
        string base64PasswordHash = keySection.GetValue<string>("PasswordHashBase64")!;
        string base64PasswordSalt = keySection.GetValue<string>("PasswordSaltBase64")!;

        byte[] passwordHash = Convert.FromBase64String(base64PasswordHash);
        byte[] passwordSalt = Convert.FromBase64String(base64PasswordSalt);

        var pbkdf2 = new Rfc2898DeriveBytes(password, passwordSalt, _iterationsCount, _hashName);
        byte[] pbkdf2Bytes = pbkdf2.GetBytes(64);

        if (!passwordHash.SequenceEqual(pbkdf2Bytes.Take(32)))
        {
            throw new InvalidOperationException("Wrong password is provided for PBKDF2");
        }

        var service = new JwtKeyAccessor()
        {
            Key = pbkdf2Bytes.TakeLast(32).ToArray()
        };

        return service;
    }

    public async Task SetNewKeyPasswordAsync(IAppSettingsService appSettingsService, string password)
    {
        byte[] newPasswordSalt = RandomNumberGenerator.GetBytes(64);
        var pbkdf2 = new Rfc2898DeriveBytes(password, newPasswordSalt, _iterationsCount, _hashName);

        byte[] pbkdf2Bytes = pbkdf2.GetBytes(64);

        string base64PasswordHash = Convert.ToBase64String(pbkdf2Bytes.AsSpan(0, 32));
        string base64PasswordSalt = Convert.ToBase64String(newPasswordSalt);

        appSettingsService.UpdateAppSetting("JwtSigningKey", "PasswordHashBase64", base64PasswordHash);
        appSettingsService.UpdateAppSetting("JwtSigningKey", "PasswordSaltBase64", base64PasswordSalt);

        await appSettingsService.SaveChangesAsync();

        Key = pbkdf2Bytes.TakeLast(32).ToArray();
    }
}
