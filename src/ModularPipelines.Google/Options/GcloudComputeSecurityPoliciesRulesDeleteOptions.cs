using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "security-policies", "rules", "delete")]
public record GcloudComputeSecurityPoliciesRulesDeleteOptions(
[property: PositionalArgument] string Priority
) : GcloudOptions
{
    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--security-policy")]
    public string? SecurityPolicy { get; set; }
}