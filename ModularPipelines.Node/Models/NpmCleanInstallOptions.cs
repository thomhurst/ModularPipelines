using ModularPipelines.Options;

namespace ModularPipelines.Node.Models;

public record NpmCleanInstallOptions : CommandEnvironmentOptions
{
    public string? InstallStrategy { get; init; }
    public IEnumerable<string>? Omit { get; init; }
}