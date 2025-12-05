using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codecommit", "merge-pull-request-by-squash")]
public record AwsCodecommitMergePullRequestBySquashOptions(
[property: CliOption("--pull-request-id")] string PullRequestId,
[property: CliOption("--repository-name")] string RepositoryName
) : AwsOptions
{
    [CliOption("--source-commit-id")]
    public string? SourceCommitId { get; set; }

    [CliOption("--conflict-detail-level")]
    public string? ConflictDetailLevel { get; set; }

    [CliOption("--conflict-resolution-strategy")]
    public string? ConflictResolutionStrategy { get; set; }

    [CliOption("--commit-message")]
    public string? CommitMessage { get; set; }

    [CliOption("--author-name")]
    public string? AuthorName { get; set; }

    [CliOption("--email")]
    public string? Email { get; set; }

    [CliOption("--conflict-resolution")]
    public string? ConflictResolution { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}