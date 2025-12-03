using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "lb", "rule", "update")]
public record AzNetworkLbRuleUpdateOptions(
[property: CliOption("--lb-name")] string LbName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--backend-pool-name")]
    public string? BackendPoolName { get; set; }

    [CliOption("--backend-port")]
    public string? BackendPort { get; set; }

    [CliFlag("--disable-outbound-snat")]
    public bool? DisableOutboundSnat { get; set; }

    [CliFlag("--enable-floating-ip")]
    public bool? EnableFloatingIp { get; set; }

    [CliFlag("--enable-tcp-reset")]
    public bool? EnableTcpReset { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--frontend-ip")]
    public string? FrontendIp { get; set; }

    [CliOption("--frontend-port")]
    public string? FrontendPort { get; set; }

    [CliOption("--idle-timeout")]
    public string? IdleTimeout { get; set; }

    [CliOption("--load-distribution")]
    public string? LoadDistribution { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--probe")]
    public string? Probe { get; set; }

    [CliOption("--protocol")]
    public string? Protocol { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }
}