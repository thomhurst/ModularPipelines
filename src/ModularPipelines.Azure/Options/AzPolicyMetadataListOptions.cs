using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("policy", "metadata", "list")]
public record AzPolicyMetadataListOptions : AzOptions
{
    [CliOption("--top")]
    public string? Top { get; set; }
}