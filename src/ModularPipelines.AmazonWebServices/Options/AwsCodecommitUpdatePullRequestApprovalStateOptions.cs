using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codecommit", "update-pull-request-approval-state")]
public record AwsCodecommitUpdatePullRequestApprovalStateOptions(
[property: CliOption("--pull-request-id")] string PullRequestId,
[property: CliOption("--revision-id")] string RevisionId,
[property: CliOption("--approval-state")] string ApprovalState
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}