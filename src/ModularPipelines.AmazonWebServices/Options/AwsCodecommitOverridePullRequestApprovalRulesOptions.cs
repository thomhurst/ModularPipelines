using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codecommit", "override-pull-request-approval-rules")]
public record AwsCodecommitOverridePullRequestApprovalRulesOptions(
[property: CliOption("--pull-request-id")] string PullRequestId,
[property: CliOption("--revision-id")] string RevisionId,
[property: CliOption("--override-status")] string OverrideStatus
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}