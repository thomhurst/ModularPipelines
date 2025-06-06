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
    public virtual string? ToFormat { get; set; }

    [BooleanCommandSwitch("--includeall")]
    public virtual bool? Includeall { get; set; }

    [BooleanCommandSwitch("--ignore-dependencies")]
    public virtual bool? IgnoreDependencies { get; set; }

    [BooleanCommandSwitch("--force-self-service")]
    public virtual bool? ForceSelfService { get; set; }
}