using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codecommit", "merge-pull-request-by-fast-forward")]
public record AwsCodecommitMergePullRequestByFastForwardOptions(
[property: CliOption("--pull-request-id")] string PullRequestId,
[property: CliOption("--repository-name")] string RepositoryName
) : AwsOptions
{
    [CliOption("--source-commit-id")]
    public string? SourceCommitId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}