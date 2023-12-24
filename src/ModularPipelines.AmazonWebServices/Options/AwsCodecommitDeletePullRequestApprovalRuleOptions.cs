using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codecommit", "delete-pull-request-approval-rule")]
public record AwsCodecommitDeletePullRequestApprovalRuleOptions(
[property: CommandSwitch("--pull-request-id")] string PullRequestId,
[property: CommandSwitch("--approval-rule-name")] string ApprovalRuleName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}