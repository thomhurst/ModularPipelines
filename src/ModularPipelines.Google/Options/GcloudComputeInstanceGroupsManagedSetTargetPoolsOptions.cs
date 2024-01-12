using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "instance-groups", "managed", "set-target-pools")]
public record GcloudComputeInstanceGroupsManagedSetTargetPoolsOptions(
[property: PositionalArgument] string Name,
[property: CommandSwitch("--target-pools")] string[] TargetPools
) : GcloudOptions
{
    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}