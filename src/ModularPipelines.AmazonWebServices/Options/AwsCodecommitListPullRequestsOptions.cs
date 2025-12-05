using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codecommit", "list-pull-requests")]
public record AwsCodecommitListPullRequestsOptions(
[property: CliOption("--repository-name")] string RepositoryName
) : AwsOptions
{
    [CliOption("--author-arn")]
    public string? AuthorArn { get; set; }

    [CliOption("--pull-request-status")]
    public string? PullRequestStatus { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}