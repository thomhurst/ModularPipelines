using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "watcher", "show-next-hop")]
public record AzNetworkWatcherShowNextHopOptions(
[property: CommandSwitch("--dest-ip")] string DestIp,
[property: CommandSwitch("--source-ip")] string SourceIp,
[property: CommandSwitch("--vm")] string Vm
) : AzOptions
{
    [CommandSwitch("--nic")]
    public string? Nic { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}