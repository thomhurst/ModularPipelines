using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Chocolatey.Options;

[ExcludeFromCodeCoverage]
[CliCommand("feature")]
public record FeatureOptions : ChocoOptions
{
    [CliOption("--name")]
    public virtual string? Name { get; set; }
}