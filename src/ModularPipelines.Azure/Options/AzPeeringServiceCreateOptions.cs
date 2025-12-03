using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("peering", "service", "create")]
public record AzPeeringServiceCreateOptions(
[property: CliOption("--location")] string Location,
[property: CliOption("--peering-service-name")] string PeeringServiceName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--peering-service-location")]
    public string? PeeringServiceLocation { get; set; }

    [CliOption("--peering-service-provider")]
    public string? PeeringServiceProvider { get; set; }

    [CliOption("--sku")]
    public string? Sku { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}