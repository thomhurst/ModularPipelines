using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "lb", "probe", "update")]
public record AzNetworkLbProbeUpdateOptions(
[property: CliOption("--lb-name")] string LbName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--interval")]
    public int? Interval { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--number-of-probes")]
    public string? NumberOfProbes { get; set; }

    [CliOption("--path")]
    public string? Path { get; set; }

    [CliOption("--port")]
    public int? Port { get; set; }

    [CliOption("--probe-threshold")]
    public string? ProbeThreshold { get; set; }

    [CliOption("--protocol")]
    public string? Protocol { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }
}