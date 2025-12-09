using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "lb", "rule", "create")]
public record AzNetworkLbRuleCreateOptions(
[property: CliOption("--backend-port")] string BackendPort,
[property: CliOption("--frontend-port")] string FrontendPort,
[property: CliOption("--lb-name")] string LbName,
[property: CliOption("--name")] string Name,
[property: CliOption("--protocol")] string Protocol,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--backend-pool-name")]
    public string? BackendPoolName { get; set; }

    [CliFlag("--disable-outbound-snat")]
    public bool? DisableOutboundSnat { get; set; }

    [CliFlag("--enable-floating-ip")]
    public bool? EnableFloatingIp { get; set; }

    [CliFlag("--enable-tcp-reset")]
    public bool? EnableTcpReset { get; set; }

    [CliOption("--frontend-ip")]
    public string? FrontendIp { get; set; }

    [CliOption("--idle-timeout")]
    public string? IdleTimeout { get; set; }

    [CliOption("--load-distribution")]
    public string? LoadDistribution { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--probe")]
    public string? Probe { get; set; }
}