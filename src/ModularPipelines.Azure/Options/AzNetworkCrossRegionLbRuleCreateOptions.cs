using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "cross-region-lb", "rule", "create")]
public record AzNetworkCrossRegionLbRuleCreateOptions(
[property: CommandSwitch("--backend-port")] string BackendPort,
[property: CommandSwitch("--frontend-port")] string FrontendPort,
[property: CommandSwitch("--lb-name")] string LbName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--protocol")] string Protocol,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--backend-pool-name")]
    public string? BackendPoolName { get; set; }

    [BooleanCommandSwitch("--enable-floating-ip")]
    public bool? EnableFloatingIp { get; set; }

    [CommandSwitch("--frontend-ip")]
    public string? FrontendIp { get; set; }

    [CommandSwitch("--load-distribution")]
    public string? LoadDistribution { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--probe")]
    public string? Probe { get; set; }
}