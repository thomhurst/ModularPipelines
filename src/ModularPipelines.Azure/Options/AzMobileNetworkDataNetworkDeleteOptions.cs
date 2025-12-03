using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mobile-network", "data-network", "delete")]
public record AzMobileNetworkDataNetworkDeleteOptions : AzOptions
{
    [CliOption("--data-network-name")]
    public string? DataNetworkName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--mobile-network-name")]
    public string? MobileNetworkName { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}