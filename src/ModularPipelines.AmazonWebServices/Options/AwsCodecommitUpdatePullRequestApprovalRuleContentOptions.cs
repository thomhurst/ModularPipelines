using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codecommit", "update-pull-request-approval-rule-content")]
public record AwsCodecommitUpdatePullRequestApprovalRuleContentOptions(
[property: CliOption("--pull-request-id")] string PullRequestId,
[property: CliOption("--approval-rule-name")] string ApprovalRuleName,
[property: CliOption("--new-rule-content")] string NewRuleContent
) : AwsOptions
{
    [CliOption("--existing-rule-content-sha256")]
    public string? ExistingRuleContentSha256 { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}