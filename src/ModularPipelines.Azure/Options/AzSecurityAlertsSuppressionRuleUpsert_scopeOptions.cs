using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("security", "alerts-suppression-rule", "upsert_scope")]
public record AzSecurityAlertsSuppressionRuleUpsert_scopeOptions(
[property: CliOption("--field")] string Field,
[property: CliOption("--rule-name")] string RuleName
) : AzOptions
{
    [CliOption("--any-of")]
    public string? AnyOf { get; set; }

    [CliOption("--contains-substring")]
    public string? ContainsSubstring { get; set; }
}