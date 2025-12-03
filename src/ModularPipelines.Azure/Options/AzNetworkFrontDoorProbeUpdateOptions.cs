using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "front-door", "probe", "update")]
public record AzNetworkFrontDoorProbeUpdateOptions(
[property: CliOption("--front-door-name")] string FrontDoorName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--enabled")]
    public bool? Enabled { get; set; }

    [CliOption("--interval")]
    public int? Interval { get; set; }

    [CliOption("--path")]
    public string? Path { get; set; }

    [CliOption("--probeMethod")]
    public string? ProbeMethod { get; set; }

    [CliOption("--protocol")]
    public string? Protocol { get; set; }
}