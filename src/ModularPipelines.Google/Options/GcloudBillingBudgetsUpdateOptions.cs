using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("billing", "budgets", "update")]
public record GcloudBillingBudgetsUpdateOptions(
[property: PositionalArgument] string Budget,
[property: PositionalArgument] string BillingAccount
) : GcloudOptions
{
    [CommandSwitch("--credit-types-treatment")]
    public string? CreditTypesTreatment { get; set; }

    [BooleanCommandSwitch("--disable-default-iam-recipients")]
    public bool? DisableDefaultIamRecipients { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--filter-credit-types")]
    public string[]? FilterCreditTypes { get; set; }

    [CommandSwitch("--filter-labels")]
    public IEnumerable<KeyValue>? FilterLabels { get; set; }

    [CommandSwitch("--filter-projects")]
    public string[]? FilterProjects { get; set; }

    [CommandSwitch("--filter-resource-ancestors")]
    public string[]? FilterResourceAncestors { get; set; }

    [CommandSwitch("--filter-services")]
    public string[]? FilterServices { get; set; }

    [CommandSwitch("--filter-subaccounts")]
    public string[]? FilterSubaccounts { get; set; }

    [CommandSwitch("--notifications-rule-monitoring-notification-channels")]
    public string[]? NotificationsRuleMonitoringNotificationChannels { get; set; }

    [CommandSwitch("--notifications-rule-pubsub-topic")]
    public string? NotificationsRulePubsubTopic { get; set; }

    [CommandSwitch("--budget-amount")]
    public string? BudgetAmount { get; set; }

    [BooleanCommandSwitch("--last-period-amount")]
    public bool? LastPeriodAmount { get; set; }

    [CommandSwitch("--calendar-period")]
    public string? CalendarPeriod { get; set; }

    [CommandSwitch("--start-date")]
    public string? StartDate { get; set; }

    [CommandSwitch("--end-date")]
    public string? EndDate { get; set; }

    [CommandSwitch("--threshold-rules-from-file")]
    public string? ThresholdRulesFromFile { get; set; }

    [CommandSwitch("--add-threshold-rule")]
    public string[]? AddThresholdRule { get; set; }

    [BooleanCommandSwitch("percent")]
    public bool? Percent { get; set; }

    [BooleanCommandSwitch("basis")]
    public bool? Basis { get; set; }

    [BooleanCommandSwitch("--clear-threshold-rules")]
    public bool? ClearThresholdRules { get; set; }
}