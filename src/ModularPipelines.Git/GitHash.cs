namespace ModularPipelines.Git;

public record GitHash
{
    public string? Long { get; init; }

    public string? Short { get; init; }
}
