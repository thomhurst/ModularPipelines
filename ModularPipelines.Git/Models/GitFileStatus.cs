namespace ModularPipelines.Git.Models;

public record GitFileStatus
{
    public string? Status { get; set; }
    public string? File { get; set; }
}