using System.ComponentModel;

namespace ModularPipelines.Options;

public record CommandLineToolOptions(string Tool) : CommandLineOptions
{
    public IEnumerable<string>? Arguments { get; init; }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public object? ArgumentsOptionObject { get; init; }

    public IEnumerable<string>? AdditionalSwitches { get; init; }
}
