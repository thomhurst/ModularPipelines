using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "watcher", "flow-log", "show")]
public record AzNetworkWatcherFlowLogShowOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--nsg")]
    public string? Nsg { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}

