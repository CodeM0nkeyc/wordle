namespace Wordle.Api.Models;

public record Word(
    [property: JsonPropertyName("word")] string Value,
    int Length,
    string Category,
    string Language
);