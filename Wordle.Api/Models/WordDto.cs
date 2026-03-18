namespace Wordle.Api.Models;

public record WordDto(
    [property: JsonPropertyName("word")] string Value,
    int Length,
    string Category,
    string Language
);