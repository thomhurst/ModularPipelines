using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "instance-groups", "managed", "update-instances")]
public record GcloudComputeInstanceGroupsManagedUpdateInstancesOptions(
[property: PositionalArgument] string Name,
[property: BooleanCommandSwitch("--all-instances")] bool AllInstances,
[property: CommandSwitch("--instances")] string[] Instances
) : GcloudOptions
{
    [CommandSwitch("--minimal-action")]
    public string? MinimalAction { get; set; }

    [CommandSwitch("--most-disruptive-allowed-action")]
    public string? MostDisruptiveAllowedAction { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}