using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("elastic-san", "create")]
public record AzElasticSanCreateOptions(
[property: CliOption("--base-size-tib")] string BaseSizeTib,
[property: CliOption("--elastic-san-name")] string ElasticSanName,
[property: CliOption("--extended-capacity-size-tib")] string ExtendedCapacitySizeTib,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--sku")] string Sku
) : AzOptions
{
    [CliOption("--availability-zones")]
    public string? AvailabilityZones { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--public-network-access")]
    public string? PublicNetworkAccess { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}