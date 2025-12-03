using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("feature", "list")]
public record AzFeatureListOptions : AzOptions
{
    [CliOption("--namespace")]
    public string? Namespace { get; set; }
}