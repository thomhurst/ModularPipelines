using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("billing", "budgets", "create")]
public record GcloudBillingBudgetsCreateOptions(
[property: CliOption("--billing-account")] string BillingAccount,
[property: CliOption("--display-name")] string DisplayName,
[property: CliOption("--budget-amount")] string BudgetAmount,
[property: CliFlag("--last-period-amount")] bool LastPeriodAmount
) : GcloudOptions
{
    [CliOption("--credit-types-treatment")]
    public string? CreditTypesTreatment { get; set; }

    [CliFlag("--disable-default-iam-recipients")]
    public bool? DisableDefaultIamRecipients { get; set; }

    [CliOption("--filter-credit-types")]
    public string[]? FilterCreditTypes { get; set; }

    [CliOption("--filter-labels")]
    public IEnumerable<KeyValue>? FilterLabels { get; set; }

    [CliOption("--filter-projects")]
    public string[]? FilterProjects { get; set; }

    [CliOption("--filter-resource-ancestors")]
    public string[]? FilterResourceAncestors { get; set; }

    [CliOption("--filter-services")]
    public string[]? FilterServices { get; set; }

    [CliOption("--filter-subaccounts")]
    public string[]? FilterSubaccounts { get; set; }

    [CliOption("--notifications-rule-monitoring-notification-channels")]
    public string[]? NotificationsRuleMonitoringNotificationChannels { get; set; }

    [CliOption("--notifications-rule-pubsub-topic")]
    public string? NotificationsRulePubsubTopic { get; set; }

    [CliOption("--threshold-rule")]
    public string[]? ThresholdRule { get; set; }

    [CliOption("--calendar-period")]
    public string? CalendarPeriod { get; set; }

    [CliOption("--start-date")]
    public string? StartDate { get; set; }

    [CliOption("--end-date")]
    public string? EndDate { get; set; }
}