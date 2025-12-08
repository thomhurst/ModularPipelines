using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Chocolatey.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("feature")]
public record FeatureOptions : ChocoOptions
{
    [CliOption("--name")]
    public virtual string? Name { get; set; }
}