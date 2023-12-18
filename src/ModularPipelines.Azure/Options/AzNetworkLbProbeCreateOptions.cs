using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "lb", "probe", "create")]
public record AzNetworkLbProbeCreateOptions(
[property: CommandSwitch("--lb-name")] string LbName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--port")] int Port,
[property: CommandSwitch("--protocol")] string Protocol,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--interval")]
    public int? Interval { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--number-of-probes")]
    public string? NumberOfProbes { get; set; }

    [CommandSwitch("--path")]
    public string? Path { get; set; }

    [CommandSwitch("--probe-threshold")]
    public string? ProbeThreshold { get; set; }
}