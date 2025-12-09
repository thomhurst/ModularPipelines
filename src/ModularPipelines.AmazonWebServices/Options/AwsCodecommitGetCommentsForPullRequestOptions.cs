using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codecommit", "get-comments-for-pull-request")]
public record AwsCodecommitGetCommentsForPullRequestOptions(
[property: CliOption("--pull-request-id")] string PullRequestId
) : AwsOptions
{
    [CliOption("--repository-name")]
    public string? RepositoryName { get; set; }

    [CliOption("--before-commit-id")]
    public string? BeforeCommitId { get; set; }

    [CliOption("--after-commit-id")]
    public string? AfterCommitId { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}