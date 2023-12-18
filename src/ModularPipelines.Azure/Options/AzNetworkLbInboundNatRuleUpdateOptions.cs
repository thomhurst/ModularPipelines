using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "lb", "inbound-nat-rule", "update")]
public record AzNetworkLbInboundNatRuleUpdateOptions(
[property: CommandSwitch("--lb-name")] string LbName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [CommandSwitch("--backend-address-pool")]
    public string? BackendAddressPool { get; set; }

    [CommandSwitch("--backend-port")]
    public string? BackendPort { get; set; }

    [BooleanCommandSwitch("--enable-floating-ip")]
    public bool? EnableFloatingIp { get; set; }

    [BooleanCommandSwitch("--enable-tcp-reset")]
    public bool? EnableTcpReset { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

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

    [CommandSwitch("--protocol")]
    public string? Protocol { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }
}