using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("budgets", "create-budget")]
public record AwsBudgetsCreateBudgetOptions(
[property: CliOption("--account-id")] string AccountId,
[property: CliOption("--budget")] string Budget
) : AwsOptions
{
    [CliOption("--notifications-with-subscribers")]
    public string[]? NotificationsWithSubscribers { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}