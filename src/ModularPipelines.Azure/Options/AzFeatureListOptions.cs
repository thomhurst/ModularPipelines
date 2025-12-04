using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("feature", "list")]
public record AzFeatureListOptions : AzOptions
{
    [CliOption("--namespace")]
    public string? Namespace { get; set; }
}