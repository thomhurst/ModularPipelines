using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "target-pools", "update")]
public record GcloudComputeTargetPoolsUpdateOptions(
[property: PositionalArgument] string Name
) : GcloudOptions
{
    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--security-policy")]
    public string? SecurityPolicy { get; set; }

    [CommandSwitch("--security-policy-region")]
    public string? SecurityPolicyRegion { get; set; }
}