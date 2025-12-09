using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("datadog", "tag-rule", "update")]
public record AzDatadogTagRuleUpdateOptions : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--filtering-tags")]
    public string? FilteringTags { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--log-rules-filtering-tags")]
    public string? LogRulesFilteringTags { get; set; }

    [CliOption("--monitor-name")]
    public string? MonitorName { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--rule-set-name")]
    public string? RuleSetName { get; set; }

    [CliFlag("--send-aad-logs")]
    public bool? SendAadLogs { get; set; }

    [CliFlag("--send-resource-logs")]
    public bool? SendResourceLogs { get; set; }

    [CliFlag("--send-subscription-logs")]
    public bool? SendSubscriptionLogs { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}