using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codecommit", "delete-pull-request-approval-rule")]
public record AwsCodecommitDeletePullRequestApprovalRuleOptions(
[property: CliOption("--pull-request-id")] string PullRequestId,
[property: CliOption("--approval-rule-name")] string ApprovalRuleName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}