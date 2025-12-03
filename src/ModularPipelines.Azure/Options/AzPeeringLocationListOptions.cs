using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("peering", "location", "list")]
public record AzPeeringLocationListOptions(
[property: CliOption("--kind")] string Kind
) : AzOptions
{
    [CliOption("--direct-peering-type")]
    public string? DirectPeeringType { get; set; }
}