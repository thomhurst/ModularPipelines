using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "lb", "probe", "create")]
public record AzNetworkLbProbeCreateOptions(
[property: CliOption("--lb-name")] string LbName,
[property: CliOption("--name")] string Name,
[property: CliOption("--port")] int Port,
[property: CliOption("--protocol")] string Protocol,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--interval")]
    public int? Interval { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--number-of-probes")]
    public string? NumberOfProbes { get; set; }

    [CliOption("--path")]
    public string? Path { get; set; }

    [CliOption("--probe-threshold")]
    public string? ProbeThreshold { get; set; }
}