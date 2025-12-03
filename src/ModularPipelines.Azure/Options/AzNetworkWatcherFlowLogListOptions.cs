using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "watcher", "flow-log", "list")]
public record AzNetworkWatcherFlowLogListOptions : AzOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}