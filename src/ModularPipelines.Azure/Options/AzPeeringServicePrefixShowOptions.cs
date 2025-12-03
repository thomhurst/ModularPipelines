using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("peering", "service", "prefix", "show")]
public record AzPeeringServicePrefixShowOptions(
[property: CliOption("--peering-service-name")] string PeeringServiceName,
[property: CliOption("--prefix-name")] string PrefixName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--expand")]
    public string? Expand { get; set; }
}