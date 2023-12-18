using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "cross-region-lb", "address-pool", "address", "update")]
public record AzNetworkCrossRegionLbAddressPoolAddressUpdateOptions(
[property: CommandSwitch("--lb-name")] string LbName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--pool-name")] string PoolName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--frontend-ip")]
    public string? FrontendIp { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }
}