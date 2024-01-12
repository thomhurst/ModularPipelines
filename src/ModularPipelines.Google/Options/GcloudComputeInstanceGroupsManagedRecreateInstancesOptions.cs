using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "instance-groups", "managed", "recreate-instances")]
public record GcloudComputeInstanceGroupsManagedRecreateInstancesOptions(
[property: PositionalArgument] string Name,
[property: CommandSwitch("--instances")] string[] Instances
) : GcloudOptions
{
    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}