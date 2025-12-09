using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("peering", "registered-prefix", "create")]
public record AzPeeringRegisteredPrefixCreateOptions(
[property: CliOption("--peering-name")] string PeeringName,
[property: CliOption("--registered-prefix-name")] string RegisteredPrefixName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--prefix")]
    public string? Prefix { get; set; }
}