using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("billing", "budgets", "update")]
public record GcloudBillingBudgetsUpdateOptions(
[property: CliArgument] string Budget,
[property: CliArgument] string BillingAccount
) : GcloudOptions
{
    [CliOption("--credit-types-treatment")]
    public string? CreditTypesTreatment { get; set; }

    [CliFlag("--disable-default-iam-recipients")]
    public bool? DisableDefaultIamRecipients { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

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

    [CliOption("--budget-amount")]
    public string? BudgetAmount { get; set; }

    [CliFlag("--last-period-amount")]
    public bool? LastPeriodAmount { get; set; }

    [CliOption("--calendar-period")]
    public string? CalendarPeriod { get; set; }

    [CliOption("--start-date")]
    public string? StartDate { get; set; }

    [CliOption("--end-date")]
    public string? EndDate { get; set; }

    [CliOption("--threshold-rules-from-file")]
    public string? ThresholdRulesFromFile { get; set; }

    [CliOption("--add-threshold-rule")]
    public string[]? AddThresholdRule { get; set; }

    [CliFlag("percent")]
    public bool? Percent { get; set; }

    [CliFlag("basis")]
    public bool? Basis { get; set; }

    [CliFlag("--clear-threshold-rules")]
    public bool? ClearThresholdRules { get; set; }
}