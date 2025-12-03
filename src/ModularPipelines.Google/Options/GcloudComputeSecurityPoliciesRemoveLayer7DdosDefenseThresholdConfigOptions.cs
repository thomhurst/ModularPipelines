using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "security-policies", "remove-layer7-ddos-defense-threshold-config")]
public record GcloudComputeSecurityPoliciesRemoveLayer7DdosDefenseThresholdConfigOptions(
[property: CliArgument] string Name,
[property: CliOption("--threshold-config-name")] string ThresholdConfigName
) : GcloudOptions;