using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datadog", "tag-rule", "create")]
public record AzDatadogTagRuleCreateOptions(
[property: CliOption("--monitor-name")] string MonitorName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--rule-set-name")] string RuleSetName
) : AzOptions
{
    [CliOption("--filtering-tags")]
    public string? FilteringTags { get; set; }

    [CliOption("--log-rules-filtering-tags")]
    public string? LogRulesFilteringTags { get; set; }

    [CliFlag("--send-aad-logs")]
    public bool? SendAadLogs { get; set; }

    [CliFlag("--send-resource-logs")]
    public bool? SendResourceLogs { get; set; }

    [CliFlag("--send-subscription-logs")]
    public bool? SendSubscriptionLogs { get; set; }
}