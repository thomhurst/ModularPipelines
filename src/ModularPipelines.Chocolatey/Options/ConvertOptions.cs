using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Chocolatey.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("convert")]
public record ConvertOptions(
    [property: CliArgument] string Pkg
) : ChocoOptions
{
    [CliOption("--to-format")]
    public virtual string? ToFormat { get; set; }

    [CliFlag("--includeall")]
    public virtual bool? Includeall { get; set; }

    [CliFlag("--ignore-dependencies")]
    public virtual bool? IgnoreDependencies { get; set; }

    [CliFlag("--force-self-service")]
    public virtual bool? ForceSelfService { get; set; }
}