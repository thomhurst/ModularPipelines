using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("security", "alerts-suppression-rule", "list")]
public record AzSecurityAlertsSuppressionRuleListOptions : AzOptions;