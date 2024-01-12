using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("alloydb", "instances", "describe")]
public record GcloudAlloydbInstancesDescribeOptions(
[property: PositionalArgument] string Instance,
[property: CommandSwitch("--cluster")] string Cluster,
[property: CommandSwitch("--region")] string Region
) : GcloudOptions
{
    [CommandSwitch("--view")]
    public string? View { get; set; }
}