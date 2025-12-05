using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codecommit", "update-pull-request-status")]
public record AwsCodecommitUpdatePullRequestStatusOptions(
[property: CliOption("--pull-request-id")] string PullRequestId,
[property: CliOption("--pull-request-status")] string PullRequestStatus
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}