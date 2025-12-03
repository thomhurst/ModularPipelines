using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("security", "alerts-suppression-rule", "update")]
public record AzSecurityAlertsSuppressionRuleUpdateOptions(
[property: CliOption("--alert-type")] string AlertType,
[property: CliOption("--reason")] string Reason,
[property: CliOption("--rule-name")] string RuleName,
[property: CliOption("--state")] string State
) : AzOptions
{
    [CliOption("--comment")]
    public string? Comment { get; set; }

    [CliOption("--expiration-date-utc")]
    public string? ExpirationDateUtc { get; set; }
}