using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "instance-groups", "managed", "instance-configs", "delete")]
public record GcloudComputeInstanceGroupsManagedInstanceConfigsDeleteOptions(
[property: PositionalArgument] string Name,
[property: CommandSwitch("--instances")] string[] Instances
) : GcloudOptions
{
    [CommandSwitch("--instance-update-minimal-action")]
    public string? InstanceUpdateMinimalAction { get; set; }

    [BooleanCommandSwitch("--update-instance")]
    public bool? UpdateInstance { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}