using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "target-pools", "remove-instances")]
public record GcloudComputeTargetPoolsRemoveInstancesOptions(
[property: PositionalArgument] string Name,
[property: CommandSwitch("--instances")] string[] Instances
) : GcloudOptions
{
    [CommandSwitch("--instances-zone")]
    public string? InstancesZone { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}