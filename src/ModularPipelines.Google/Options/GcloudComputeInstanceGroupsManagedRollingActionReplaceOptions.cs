using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "instance-groups", "managed", "rolling-action", "replace")]
public record GcloudComputeInstanceGroupsManagedRollingActionReplaceOptions(
[property: PositionalArgument] string Name
) : GcloudOptions
{
    [CommandSwitch("--max-surge")]
    public string? MaxSurge { get; set; }

    [CommandSwitch("--max-unavailable")]
    public string? MaxUnavailable { get; set; }

    [CommandSwitch("--replacement-method")]
    public string? ReplacementMethod { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}