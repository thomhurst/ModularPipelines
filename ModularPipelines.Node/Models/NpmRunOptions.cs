using ModularPipelines.Options;

namespace ModularPipelines.Node.Models;

public record NpmRunOptions : CommandEnvironmentOptions
{
    public string Target { get; init; }
    public IEnumerable<string>? Arguments { get; init; }
}