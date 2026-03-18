namespace Wordle.Api.Models;

public static class WordleDefaults
{
    public const int Length = 5;

    public static DateTime StartRefreshUtc => new DateTime(
        DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day + 1, 10, 0, 0);
}
