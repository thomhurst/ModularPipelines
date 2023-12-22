using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Chocolatey.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("convert")]
public record ConvertOptions(
    [property: PositionalArgument] string Pkg
) : ChocoOptions
{
    [CommandSwitch("--to-format")]
    public string? ToFormat { get; set; }

    [BooleanCommandSwitch("--includeall")]
    public bool? Includeall { get; set; }

    [BooleanCommandSwitch("--ignore-dependencies")]
    public bool? IgnoreDependencies { get; set; }

    [BooleanCommandSwitch("--force-self-service")]
    public bool? ForceSelfService { get; set; }
}