namespace ModularPipelines.Git.Models;

public record GitMessage
{
    public string? Subject { get; init; }

    public string? Body { get; init; }
}