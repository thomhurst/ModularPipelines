using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "watcher", "show-next-hop")]
public record AzNetworkWatcherShowNextHopOptions(
[property: CliOption("--dest-ip")] string DestIp,
[property: CliOption("--source-ip")] string SourceIp,
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