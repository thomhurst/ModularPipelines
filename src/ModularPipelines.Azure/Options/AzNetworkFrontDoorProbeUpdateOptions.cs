using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "front-door", "probe", "update")]
public record AzNetworkFrontDoorProbeUpdateOptions(
[property: CommandSwitch("--front-door-name")] string FrontDoorName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--enabled")]
    public bool? Enabled { get; set; }

    [CommandSwitch("--interval")]
    public int? Interval { get; set; }

    [CommandSwitch("--path")]
    public string? Path { get; set; }

    [CommandSwitch("--probeMethod")]
    public string? ProbeMethod { get; set; }

    [CommandSwitch("--protocol")]
    public string? Protocol { get; set; }
}