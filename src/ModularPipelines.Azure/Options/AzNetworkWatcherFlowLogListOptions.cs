using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "watcher", "flow-log", "list")]
public record AzNetworkWatcherFlowLogListOptions : AzOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}