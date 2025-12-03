using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "security-policies", "add-layer7-ddos-defense-threshold-config")]
public record GcloudComputeSecurityPoliciesAddLayer7DdosDefenseThresholdConfigOptions(
[property: CliArgument] string Name,
[property: CliOption("--threshold-config-name")] string ThresholdConfigName
) : GcloudOptions
{
    [CliOption("--auto-deploy-confidence-threshold")]
    public string? AutoDeployConfidenceThreshold { get; set; }

    [CliOption("--auto-deploy-expiration-sec")]
    public string? AutoDeployExpirationSec { get; set; }

    [CliOption("--auto-deploy-impacted-baseline-threshold")]
    public string? AutoDeployImpactedBaselineThreshold { get; set; }

    [CliOption("--auto-deploy-load-threshold")]
    public string? AutoDeployLoadThreshold { get; set; }
}