using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "instance-groups", "managed", "rolling-action", "restart")]
public record GcloudComputeInstanceGroupsManagedRollingActionRestartOptions(
[property: PositionalArgument] string Name
) : GcloudOptions
{
    [CommandSwitch("--max-unavailable")]
    public string? MaxUnavailable { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}