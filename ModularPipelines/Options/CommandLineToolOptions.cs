using System.ComponentModel;

namespace ModularPipelines.Options;

public record CommandLineToolOptions(string Tool) : CommandLineOptions
{
    public IEnumerable<string>? Arguments { get; init; }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public object? OptionsObject { get; init; }
}
