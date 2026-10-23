namespace ModularPipelines.Git;

public record GitAuthor
{
    public string? Name { get; init; }

    public string? Email { get; init; }

    public DateTime Date { get; init; }
}
