using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("peering", "service", "location", "list")]
public record AzPeeringServiceLocationListOptions : AzOptions
{
    [CliOption("--country")]
    public int? Country { get; set; }
}