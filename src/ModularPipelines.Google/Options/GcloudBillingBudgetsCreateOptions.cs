using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("billing", "budgets", "create")]
public record GcloudBillingBudgetsCreateOptions(
[property: CommandSwitch("--billing-account")] string BillingAccount,
[property: CommandSwitch("--display-name")] string DisplayName,
[property: CommandSwitch("--budget-amount")] string BudgetAmount,
[property: BooleanCommandSwitch("--last-period-amount")] bool LastPeriodAmount
) : GcloudOptions
{
    [CommandSwitch("--credit-types-treatment")]
    public string? CreditTypesTreatment { get; set; }

    [BooleanCommandSwitch("--disable-default-iam-recipients")]
    public bool? DisableDefaultIamRecipients { get; set; }

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

    [CommandSwitch("--threshold-rule")]
    public string[]? ThresholdRule { get; set; }

    [CommandSwitch("--calendar-period")]
    public string? CalendarPeriod { get; set; }

    [CommandSwitch("--start-date")]
    public string? StartDate { get; set; }

    [CommandSwitch("--end-date")]
    public string? EndDate { get; set; }
}