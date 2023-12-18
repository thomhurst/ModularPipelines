using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datadog", "tag-rule", "create")]
public record AzDatadogTagRuleCreateOptions(
[property: CommandSwitch("--monitor-name")] string MonitorName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--rule-set-name")] string RuleSetName
) : AzOptions
{
    [CommandSwitch("--filtering-tags")]
    public string? FilteringTags { get; set; }

    [CommandSwitch("--log-rules-filtering-tags")]
    public string? LogRulesFilteringTags { get; set; }

    [BooleanCommandSwitch("--send-aad-logs")]
    public bool? SendAadLogs { get; set; }

    [BooleanCommandSwitch("--send-resource-logs")]
    public bool? SendResourceLogs { get; set; }

    [BooleanCommandSwitch("--send-subscription-logs")]
    public bool? SendSubscriptionLogs { get; set; }
}

