using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("logz", "rule", "update")]
public record AzLogzRuleUpdateOptions : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [CommandSwitch("--filtering-tags")]
    public string? FilteringTags { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--monitor-name")]
    public string? MonitorName { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--rule-set-name")]
    public string? RuleSetName { get; set; }

    [BooleanCommandSwitch("--send-aad-logs")]
    public bool? SendAadLogs { get; set; }

    [BooleanCommandSwitch("--send-activity-logs")]
    public bool? SendActivityLogs { get; set; }

    [BooleanCommandSwitch("--send-subscription-logs")]
    public bool? SendSubscriptionLogs { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}

