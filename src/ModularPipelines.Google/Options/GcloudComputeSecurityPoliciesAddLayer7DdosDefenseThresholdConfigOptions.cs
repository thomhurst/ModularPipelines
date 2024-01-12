using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "security-policies", "add-layer7-ddos-defense-threshold-config")]
public record GcloudComputeSecurityPoliciesAddLayer7DdosDefenseThresholdConfigOptions(
[property: PositionalArgument] string Name,
[property: CommandSwitch("--threshold-config-name")] string ThresholdConfigName
) : GcloudOptions
{
    [CommandSwitch("--auto-deploy-confidence-threshold")]
    public string? AutoDeployConfidenceThreshold { get; set; }

    [CommandSwitch("--auto-deploy-expiration-sec")]
    public string? AutoDeployExpirationSec { get; set; }

    [CommandSwitch("--auto-deploy-impacted-baseline-threshold")]
    public string? AutoDeployImpactedBaselineThreshold { get; set; }

    [CommandSwitch("--auto-deploy-load-threshold")]
    public string? AutoDeployLoadThreshold { get; set; }
}