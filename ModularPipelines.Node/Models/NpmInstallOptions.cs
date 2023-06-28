using ModularPipelines.Options;

namespace ModularPipelines.Node.Models;

public record NpmInstallOptions : CommandEnvironmentOptions
{
    public string? Target { get; init; }
    public bool Global { get; init; }
    public bool DryRun { get; init; }
    public bool Force { get; init; }
}