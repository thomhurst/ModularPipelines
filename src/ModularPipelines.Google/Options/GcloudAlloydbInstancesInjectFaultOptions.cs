using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("alloydb", "instances", "inject-fault")]
public record GcloudAlloydbInstancesInjectFaultOptions(
[property: PositionalArgument] string Instance,
[property: CommandSwitch("--cluster")] string Cluster,
[property: CommandSwitch("--fault-type")] string FaultType,
[property: CommandSwitch("--region")] string Region
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }
}