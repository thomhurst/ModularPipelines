namespace ModularPipelines.Git.Models;

public record GitMessage
{
    public string? Subject { get; set; }

    public string? Body { get; set; }
}