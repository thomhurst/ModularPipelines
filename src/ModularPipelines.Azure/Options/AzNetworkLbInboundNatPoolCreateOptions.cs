using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "lb", "inbound-nat-pool", "create")]
public record AzNetworkLbInboundNatPoolCreateOptions(
[property: CliOption("--backend-port")] string BackendPort,
[property: CliOption("--frontend-port-range-end")] string FrontendPortRangeEnd,
[property: CliOption("--frontend-port-range-start")] string FrontendPortRangeStart,
[property: CliOption("--lb-name")] string LbName,
[property: CliOption("--name")] string Name,
[property: CliOption("--protocol")] string Protocol,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--enable-floating-ip")]
    public bool? EnableFloatingIp { get; set; }

    [CliFlag("--enable-tcp-reset")]
    public bool? EnableTcpReset { get; set; }

    [CliOption("--frontend-ip")]
    public string? FrontendIp { get; set; }

    [CliOption("--idle-timeout")]
    public string? IdleTimeout { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}