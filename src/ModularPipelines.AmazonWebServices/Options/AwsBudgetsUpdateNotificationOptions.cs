using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("budgets", "update-notification")]
public record AwsBudgetsUpdateNotificationOptions(
[property: CliOption("--account-id")] string AccountId,
[property: CliOption("--budget-name")] string BudgetName,
[property: CliOption("--old-notification")] string OldNotification,
[property: CliOption("--new-notification")] string NewNotification
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}