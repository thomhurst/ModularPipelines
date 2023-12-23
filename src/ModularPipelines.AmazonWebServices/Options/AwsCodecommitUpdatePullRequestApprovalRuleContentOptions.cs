using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codecommit", "update-pull-request-approval-rule-content")]
public record AwsCodecommitUpdatePullRequestApprovalRuleContentOptions(
[property: CommandSwitch("--pull-request-id")] string PullRequestId,
[property: CommandSwitch("--approval-rule-name")] string ApprovalRuleName,
[property: CommandSwitch("--new-rule-content")] string NewRuleContent
) : AwsOptions
{
    [CommandSwitch("--existing-rule-content-sha256")]
    public string? ExistingRuleContentSha256 { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}