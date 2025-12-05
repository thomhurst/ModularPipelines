using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("budgets", "execute-budget-action")]
public record AwsBudgetsExecuteBudgetActionOptions(
[property: CliOption("--account-id")] string AccountId,
[property: CliOption("--budget-name")] string BudgetName,
[property: CliOption("--action-id")] string ActionId,
[property: CliOption("--execution-type")] string ExecutionType
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}