using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codecommit", "post-comment-for-pull-request")]
public record AwsCodecommitPostCommentForPullRequestOptions(
[property: CliOption("--pull-request-id")] string PullRequestId,
[property: CliOption("--repository-name")] string RepositoryName,
[property: CliOption("--before-commit-id")] string BeforeCommitId,
[property: CliOption("--after-commit-id")] string AfterCommitId,
[property: CliOption("--content")] string Content
) : AwsOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}