using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "watcher", "test-ip-flow")]
public record AzNetworkWatcherTestIpFlowOptions(
[property: CommandSwitch("--direction")] string Direction,
[property: CommandSwitch("--local")] string Local,
[property: CommandSwitch("--protocol")] string Protocol,
[property: CommandSwitch("--remote")] string Remote,
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