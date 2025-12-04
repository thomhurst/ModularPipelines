using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("peering", "service", "prefix", "list")]
public record AzPeeringServicePrefixListOptions(
[property: CliOption("--peering-service-name")] string PeeringServiceName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--expand")]
    public string? Expand { get; set; }
}