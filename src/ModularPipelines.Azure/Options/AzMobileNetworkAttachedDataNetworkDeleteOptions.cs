using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mobile-network", "attached-data-network", "delete")]
public record AzMobileNetworkAttachedDataNetworkDeleteOptions : AzOptions
{
    [CliOption("--adn-name")]
    public string? AdnName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--pccp-name")]
    public string? PccpName { get; set; }

    [CliOption("--pcdp-name")]
    public string? PcdpName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}