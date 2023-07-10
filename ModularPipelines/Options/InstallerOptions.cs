using ModularPipelines.Attributes;

namespace ModularPipelines.Options;

public record InstallerOptions([property: PositionalArgument] string Path)
{
    public IEnumerable<string>? Arguments { get; init; }
}
