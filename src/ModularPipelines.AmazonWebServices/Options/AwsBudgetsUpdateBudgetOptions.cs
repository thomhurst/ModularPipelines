using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("budgets", "update-budget")]
public record AwsBudgetsUpdateBudgetOptions(
[property: CliOption("--account-id")] string AccountId,
[property: CliOption("--new-budget")] string NewBudget
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}