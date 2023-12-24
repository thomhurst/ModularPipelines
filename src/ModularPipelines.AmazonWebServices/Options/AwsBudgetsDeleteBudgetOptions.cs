using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("budgets", "delete-budget")]
public record AwsBudgetsDeleteBudgetOptions(
[property: CommandSwitch("--account-id")] string AccountId,
[property: CommandSwitch("--budget-name")] string BudgetName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}