using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("security", "alerts-suppression-rule", "update")]
public record AzSecurityAlertsSuppressionRuleUpdateOptions(
[property: CommandSwitch("--alert-type")] string AlertType,
[property: CommandSwitch("--reason")] string Reason,
[property: CommandSwitch("--rule-name")] string RuleName,
[property: CommandSwitch("--state")] string State
) : AzOptions
{
    [CommandSwitch("--comment")]
    public string? Comment { get; set; }

    [CommandSwitch("--expiration-date-utc")]
    public string? ExpirationDateUtc { get; set; }
}

