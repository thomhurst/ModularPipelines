using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Chocolatey.Options;

[ExcludeFromCodeCoverage]
[CliCommand("feature", "enable")]
public record FeatureEnableOptions : ChocoOptions
{
    [CliOption("--name")]
    public virtual string? Name { get; set; }
}