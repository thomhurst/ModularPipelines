using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("budgets", "describe-budget")]
public record AwsBudgetsDescribeBudgetOptions(
[property: CliOption("--account-id")] string AccountId,
[property: CliOption("--budget-name")] string BudgetName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}