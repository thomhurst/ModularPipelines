using ModularPipelines.Options;

namespace ModularPipelines.Node.Models;

public record NpmRunOptions(string Target) : CommandEnvironmentOptions
{
    public IEnumerable<string>? Arguments { get; init; }
}