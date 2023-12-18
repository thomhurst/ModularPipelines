using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "lb", "inbound-nat-pool", "create")]
public record AzNetworkLbInboundNatPoolCreateOptions(
[property: CommandSwitch("--backend-port")] string BackendPort,
[property: CommandSwitch("--frontend-port-range-end")] string FrontendPortRangeEnd,
[property: CommandSwitch("--frontend-port-range-start")] string FrontendPortRangeStart,
[property: CommandSwitch("--lb-name")] string LbName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--protocol")] string Protocol,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--enable-floating-ip")]
    public bool? EnableFloatingIp { get; set; }

    [BooleanCommandSwitch("--enable-tcp-reset")]
    public bool? EnableTcpReset { get; set; }

    [CommandSwitch("--frontend-ip")]
    public string? FrontendIp { get; set; }

    [CommandSwitch("--idle-timeout")]
    public string? IdleTimeout { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }
}