using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "lb", "inbound-nat-rule", "create")]
public record AzNetworkLbInboundNatRuleCreateOptions(
[property: CliOption("--backend-port")] string BackendPort,
[property: CliOption("--lb-name")] string LbName,
[property: CliOption("--name")] string Name,
[property: CliOption("--protocol")] string Protocol,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--backend-address-pool")]
    public string? BackendAddressPool { get; set; }

    [CliFlag("--enable-floating-ip")]
    public bool? EnableFloatingIp { get; set; }

    [CliFlag("--enable-tcp-reset")]
    public bool? EnableTcpReset { get; set; }

    [CliOption("--frontend-ip")]
    public string? FrontendIp { get; set; }

    [CliOption("--frontend-port")]
    public string? FrontendPort { get; set; }

    [CliOption("--frontend-port-range-end")]
    public string? FrontendPortRangeEnd { get; set; }

    [CliOption("--frontend-port-range-start")]
    public string? FrontendPortRangeStart { get; set; }

    [CliOption("--idle-timeout")]
    public string? IdleTimeout { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}