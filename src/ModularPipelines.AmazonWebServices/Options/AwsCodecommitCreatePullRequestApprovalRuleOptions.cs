using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codecommit", "create-pull-request-approval-rule")]
public record AwsCodecommitCreatePullRequestApprovalRuleOptions(
[property: CommandSwitch("--pull-request-id")] string PullRequestId,
[property: CommandSwitch("--approval-rule-name")] string ApprovalRuleName,
[property: CommandSwitch("--approval-rule-content")] string ApprovalRuleContent
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}