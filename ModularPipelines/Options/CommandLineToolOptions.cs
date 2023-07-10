using System.ComponentModel;
using ModularPipelines.Attributes;

namespace ModularPipelines.Options;

public record CommandLineToolOptions([property: PositionalArgument] string Tool) : CommandLineOptions
{
    public IEnumerable<string>? Arguments { get; init; }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public object? OptionsObject { get; init; }
    
    public IEnumerable<string>? RunSettings { get; init; }
}
