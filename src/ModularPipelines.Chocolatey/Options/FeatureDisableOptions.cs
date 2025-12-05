using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Chocolatey.Options;

[ExcludeFromCodeCoverage]
[CliCommand("feature", "disable")]
public record FeatureDisableOptions : ChocoOptions
{
    [CliOption("--name")]
    public virtual string? Name { get; set; }
}