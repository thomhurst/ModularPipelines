using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("security", "alerts-suppression-rule", "list")]
public record AzSecurityAlertsSuppressionRuleListOptions(
[property: CommandSwitch("--rule-name")] string RuleName
) : AzOptions
{
    [CommandSwitch("--comment")]
    public string? Comment { get; set; }

    [CommandSwitch("--expiration-date-utc")]
    public string? ExpirationDateUtc { get; set; }
}