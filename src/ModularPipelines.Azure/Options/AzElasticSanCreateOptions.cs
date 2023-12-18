using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elastic-san", "create")]
public record AzElasticSanCreateOptions(
[property: CommandSwitch("--base-size-tib")] string BaseSizeTib,
[property: CommandSwitch("--elastic-san-name")] string ElasticSanName,
[property: CommandSwitch("--extended-capacity-size-tib")] string ExtendedCapacitySizeTib,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--sku")] string Sku
) : AzOptions
{
    [CommandSwitch("--availability-zones")]
    public string? AvailabilityZones { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--public-network-access")]
    public string? PublicNetworkAccess { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}