using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("peering", "service", "update")]
public record AzPeeringServiceUpdateOptions(
[property: CliOption("--peering-service-name")] string PeeringServiceName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--tags")]
    public string? Tags { get; set; }
}