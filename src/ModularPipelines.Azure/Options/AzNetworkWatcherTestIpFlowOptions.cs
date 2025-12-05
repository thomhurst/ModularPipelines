using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "watcher", "test-ip-flow")]
public record AzNetworkWatcherTestIpFlowOptions(
[property: CliOption("--direction")] string Direction,
[property: CliOption("--local")] string Local,
[property: CliOption("--protocol")] string Protocol,
[property: CliOption("--remote")] string Remote,
[property: CliOption("--vm")] string Vm
) : AzOptions
{
    [CliOption("--nic")]
    public string? Nic { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}