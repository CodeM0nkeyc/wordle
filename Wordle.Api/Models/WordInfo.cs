namespace Wordle.Api.Models;

public record WordInfo(
    string Value,
    int Length,
    string Category,
    string Language
);