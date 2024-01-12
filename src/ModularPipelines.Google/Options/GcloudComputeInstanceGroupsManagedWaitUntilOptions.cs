using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "instance-groups", "managed", "wait-until")]
public record GcloudComputeInstanceGroupsManagedWaitUntilOptions(
[property: PositionalArgument] string Name,
[property: BooleanCommandSwitch("--stable")] bool Stable,
[property: BooleanCommandSwitch("--version-target-reached")] bool VersionTargetReached
) : GcloudOptions
{
    [CommandSwitch("--timeout")]
    public string? Timeout { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}