using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "lb", "inbound-nat-rule", "create")]
public record AzNetworkLbInboundNatRuleCreateOptions(
[property: CommandSwitch("--backend-port")] string BackendPort,
[property: CommandSwitch("--lb-name")] string LbName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--protocol")] string Protocol,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--backend-address-pool")]
    public string? BackendAddressPool { get; set; }

    [BooleanCommandSwitch("--enable-floating-ip")]
    public bool? EnableFloatingIp { get; set; }

    [BooleanCommandSwitch("--enable-tcp-reset")]
    public bool? EnableTcpReset { get; set; }

    [CommandSwitch("--frontend-ip")]
    public string? FrontendIp { get; set; }

    [CommandSwitch("--frontend-port")]
    public string? FrontendPort { get; set; }

    [CommandSwitch("--frontend-port-range-end")]
    public string? FrontendPortRangeEnd { get; set; }

    [CommandSwitch("--frontend-port-range-start")]
    public string? FrontendPortRangeStart { get; set; }

    [CommandSwitch("--idle-timeout")]
    public string? IdleTimeout { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }
}