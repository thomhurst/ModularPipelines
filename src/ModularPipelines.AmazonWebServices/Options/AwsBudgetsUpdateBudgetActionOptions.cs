using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("budgets", "update-budget-action")]
public record AwsBudgetsUpdateBudgetActionOptions(
[property: CommandSwitch("--account-id")] string AccountId,
[property: CommandSwitch("--budget-name")] string BudgetName,
[property: CommandSwitch("--action-id")] string ActionId
) : AwsOptions
{
    [CommandSwitch("--notification-type")]
    public string? NotificationType { get; set; }

    [CommandSwitch("--action-threshold")]
    public string? ActionThreshold { get; set; }

    [CommandSwitch("--definition")]
    public string? Definition { get; set; }

    [CommandSwitch("--execution-role-arn")]
    public string? ExecutionRoleArn { get; set; }

    [CommandSwitch("--approval-model")]
    public string? ApprovalModel { get; set; }

    [CommandSwitch("--subscribers")]
    public string[]? Subscribers { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}