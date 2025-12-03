using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("security", "alerts-suppression-rule", "show")]
public record AzSecurityAlertsSuppressionRuleShowOptions(
[property: CliOption("--rule-name")] string RuleName
) : AzOptions;