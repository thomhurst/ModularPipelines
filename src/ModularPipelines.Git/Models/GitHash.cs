namespace ModularPipelines.Git.Models;

public record GitHash
{
    public string? Long { get; set; }
    public string? Short { get; set; }
}
