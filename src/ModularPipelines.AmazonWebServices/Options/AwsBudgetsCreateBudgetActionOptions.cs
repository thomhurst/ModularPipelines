using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("budgets", "create-budget-action")]
public record AwsBudgetsCreateBudgetActionOptions(
[property: CommandSwitch("--account-id")] string AccountId,
[property: CommandSwitch("--budget-name")] string BudgetName,
[property: CommandSwitch("--notification-type")] string NotificationType,
[property: CommandSwitch("--action-type")] string ActionType,
[property: CommandSwitch("--action-threshold")] string ActionThreshold,
[property: CommandSwitch("--definition")] string Definition,
[property: CommandSwitch("--execution-role-arn")] string ExecutionRoleArn,
[property: CommandSwitch("--approval-model")] string ApprovalModel,
[property: CommandSwitch("--subscribers")] string[] Subscribers
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}