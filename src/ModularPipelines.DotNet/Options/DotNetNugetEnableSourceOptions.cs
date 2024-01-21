using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[ExcludeFromCodeCoverage]
public record DotNetNugetEnableSourceOptions : DotNetOptions
{
    [PositionalArgument(PlaceholderName = "<NAME>")]
    public string? Name { get; set; }

    [CommandSwitch("--configfile")]
    public string? Configfile { get; set; }
}
