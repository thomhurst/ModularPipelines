using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("logz", "sub-rule", "create")]
public record AzLogzSubRuleCreateOptions(
[property: CommandSwitch("--monitor-name")] string MonitorName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--rule-set-name")] string RuleSetName,
[property: CommandSwitch("--sub-account-name")] int SubAccountName
) : AzOptions
{
    [CommandSwitch("--filtering-tags")]
    public string? FilteringTags { get; set; }

    [BooleanCommandSwitch("--send-aad-logs")]
    public bool? SendAadLogs { get; set; }

    [BooleanCommandSwitch("--send-activity-logs")]
    public bool? SendActivityLogs { get; set; }

    [BooleanCommandSwitch("--send-subscription-logs")]
    public bool? SendSubscriptionLogs { get; set; }
}

