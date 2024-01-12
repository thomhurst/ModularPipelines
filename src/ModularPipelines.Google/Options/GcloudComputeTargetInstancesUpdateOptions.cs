using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "target-instances", "update")]
public record GcloudComputeTargetInstancesUpdateOptions(
[property: PositionalArgument] string Name
) : GcloudOptions
{
    [CommandSwitch("--security-policy")]
    public string? SecurityPolicy { get; set; }

    [CommandSwitch("--security-policy-region")]
    public string? SecurityPolicyRegion { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}