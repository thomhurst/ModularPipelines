using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "front-door", "probe", "create")]
public record AzNetworkFrontDoorProbeCreateOptions(
[property: CommandSwitch("--front-door-name")] string FrontDoorName,
[property: CommandSwitch("--interval")] int Interval,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--path")] string Path,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--enabled")]
    public bool? Enabled { get; set; }

    [CommandSwitch("--probeMethod")]
    public string? ProbeMethod { get; set; }

    [CommandSwitch("--protocol")]
    public string? Protocol { get; set; }
}

