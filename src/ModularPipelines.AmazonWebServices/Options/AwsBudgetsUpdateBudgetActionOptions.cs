using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("budgets", "update-budget-action")]
public record AwsBudgetsUpdateBudgetActionOptions(
[property: CliOption("--account-id")] string AccountId,
[property: CliOption("--budget-name")] string BudgetName,
[property: CliOption("--action-id")] string ActionId
) : AwsOptions
{
    [CliOption("--notification-type")]
    public string? NotificationType { get; set; }

    [CliOption("--action-threshold")]
    public string? ActionThreshold { get; set; }

    [CliOption("--definition")]
    public string? Definition { get; set; }

    [CliOption("--execution-role-arn")]
    public string? ExecutionRoleArn { get; set; }

    [CliOption("--approval-model")]
    public string? ApprovalModel { get; set; }

    [CliOption("--subscribers")]
    public string[]? Subscribers { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}