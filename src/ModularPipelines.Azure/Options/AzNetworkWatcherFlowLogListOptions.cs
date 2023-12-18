using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "watcher", "flow-log", "list")]
public record AzNetworkWatcherFlowLogListOptions : AzOptions
{
    [CommandSwitch("--location")]
    public string? Location { get; set; }
}