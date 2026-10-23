namespace ModularPipelines.Git;

public record GitMessage
{
    public string? Subject { get; init; }

    public string? Body { get; init; }
}
