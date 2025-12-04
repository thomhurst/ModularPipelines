using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("logz", "rule", "create")]
public record AzLogzRuleCreateOptions(
[property: CliOption("--monitor-name")] string MonitorName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--rule-set-name")] string RuleSetName
) : AzOptions
{
    [CliOption("--filtering-tags")]
    public string? FilteringTags { get; set; }

    [CliFlag("--send-aad-logs")]
    public bool? SendAadLogs { get; set; }

    [CliFlag("--send-activity-logs")]
    public bool? SendActivityLogs { get; set; }

    [CliFlag("--send-subscription-logs")]
    public bool? SendSubscriptionLogs { get; set; }
}