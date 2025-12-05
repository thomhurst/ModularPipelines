using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("budgets", "update-subscriber")]
public record AwsBudgetsUpdateSubscriberOptions(
[property: CliOption("--account-id")] string AccountId,
[property: CliOption("--budget-name")] string BudgetName,
[property: CliOption("--notification")] string Notification,
[property: CliOption("--old-subscriber")] string OldSubscriber,
[property: CliOption("--new-subscriber")] string NewSubscriber
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}