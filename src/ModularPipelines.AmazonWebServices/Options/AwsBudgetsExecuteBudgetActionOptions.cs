using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("budgets", "execute-budget-action")]
public record AwsBudgetsExecuteBudgetActionOptions(
[property: CommandSwitch("--account-id")] string AccountId,
[property: CommandSwitch("--budget-name")] string BudgetName,
[property: CommandSwitch("--action-id")] string ActionId,
[property: CommandSwitch("--execution-type")] string ExecutionType
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}