using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("peering", "service", "prefix", "update")]
public record AzPeeringServicePrefixUpdateOptions(
[property: CliOption("--peering-service-name")] string PeeringServiceName,
[property: CliOption("--prefix-name")] string PrefixName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--peering-service-prefix-key")]
    public string? PeeringServicePrefixKey { get; set; }

    [CliOption("--prefix")]
    public string? Prefix { get; set; }
}