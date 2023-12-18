using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "lb", "rule", "create")]
public record AzNetworkLbRuleCreateOptions(
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

    [BooleanCommandSwitch("--disable-outbound-snat")]
    public bool? DisableOutboundSnat { get; set; }

    [BooleanCommandSwitch("--enable-floating-ip")]
    public bool? EnableFloatingIp { get; set; }

    [BooleanCommandSwitch("--enable-tcp-reset")]
    public bool? EnableTcpReset { get; set; }

    [CommandSwitch("--frontend-ip")]
    public string? FrontendIp { get; set; }

    [CommandSwitch("--idle-timeout")]
    public string? IdleTimeout { get; set; }

    [CommandSwitch("--load-distribution")]
    public string? LoadDistribution { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--probe")]
    public string? Probe { get; set; }
}