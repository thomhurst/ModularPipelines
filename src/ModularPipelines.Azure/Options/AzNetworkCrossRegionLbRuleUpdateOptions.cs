using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "cross-region-lb", "rule", "update")]
public record AzNetworkCrossRegionLbRuleUpdateOptions(
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

    [CliFlag("--enable-floating-ip")]
    public bool? EnableFloatingIp { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--frontend-ip")]
    public string? FrontendIp { get; set; }

    [CliOption("--frontend-port")]
    public string? FrontendPort { get; set; }

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