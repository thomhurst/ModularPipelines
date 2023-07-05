using ModularPipelines.Options;

namespace ModularPipelines.Node.Models;

public record NpmRunOptions(string Target) : CommandLineOptions
{
    public IEnumerable<string>? Arguments { get; init; }
}