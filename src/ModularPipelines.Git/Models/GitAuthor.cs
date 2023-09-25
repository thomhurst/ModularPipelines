namespace ModularPipelines.Git.Models;

public record GitAuthor
{
    public string? Name { get; set; }

    public string? Email { get; set; }

    public DateTime Date { get; set; }
}
