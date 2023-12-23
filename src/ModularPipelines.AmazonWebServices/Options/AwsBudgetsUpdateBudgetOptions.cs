using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("budgets", "update-budget")]
public record AwsBudgetsUpdateBudgetOptions(
[property: CommandSwitch("--account-id")] string AccountId,
[property: CommandSwitch("--new-budget")] string NewBudget
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}