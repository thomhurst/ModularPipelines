using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "front-door", "probe", "create")]
public record AzNetworkFrontDoorProbeCreateOptions(
[property: CliOption("--front-door-name")] string FrontDoorName,
[property: CliOption("--interval")] int Interval,
[property: CliOption("--name")] string Name,
[property: CliOption("--path")] string Path,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--enabled")]
    public bool? Enabled { get; set; }

    [CliOption("--probeMethod")]
    public string? ProbeMethod { get; set; }

    [CliOption("--protocol")]
    public string? Protocol { get; set; }
}