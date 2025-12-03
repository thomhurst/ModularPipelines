using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "watcher", "show-topology")]
public record AzNetworkWatcherShowTopologyOptions : AzOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subnet")]
    public string? Subnet { get; set; }

    [CliOption("--vnet")]
    public string? Vnet { get; set; }
}