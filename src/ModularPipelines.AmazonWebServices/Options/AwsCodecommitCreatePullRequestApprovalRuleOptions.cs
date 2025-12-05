using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codecommit", "create-pull-request-approval-rule")]
public record AwsCodecommitCreatePullRequestApprovalRuleOptions(
[property: CliOption("--pull-request-id")] string PullRequestId,
[property: CliOption("--approval-rule-name")] string ApprovalRuleName,
[property: CliOption("--approval-rule-content")] string ApprovalRuleContent
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}