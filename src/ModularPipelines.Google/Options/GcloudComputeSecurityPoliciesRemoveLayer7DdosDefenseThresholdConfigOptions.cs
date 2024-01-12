using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "security-policies", "remove-layer7-ddos-defense-threshold-config")]
public record GcloudComputeSecurityPoliciesRemoveLayer7DdosDefenseThresholdConfigOptions(
[property: PositionalArgument] string Name,
[property: CommandSwitch("--threshold-config-name")] string ThresholdConfigName
) : GcloudOptions;