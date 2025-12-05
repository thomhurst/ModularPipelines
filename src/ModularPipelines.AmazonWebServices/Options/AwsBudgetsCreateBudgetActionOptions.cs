using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("budgets", "create-budget-action")]
public record AwsBudgetsCreateBudgetActionOptions(
[property: CliOption("--account-id")] string AccountId,
[property: CliOption("--budget-name")] string BudgetName,
[property: CliOption("--notification-type")] string NotificationType,
[property: CliOption("--action-type")] string ActionType,
[property: CliOption("--action-threshold")] string ActionThreshold,
[property: CliOption("--definition")] string Definition,
[property: CliOption("--execution-role-arn")] string ExecutionRoleArn,
[property: CliOption("--approval-model")] string ApprovalModel,
[property: CliOption("--subscribers")] string[] Subscribers
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}