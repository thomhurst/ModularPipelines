using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codecommit", "get-comments-for-compared-commit")]
public record AwsCodecommitGetCommentsForComparedCommitOptions(
[property: CliOption("--repository-name")] string RepositoryName,
[property: CliOption("--after-commit-id")] string AfterCommitId
) : AwsOptions
{
    [CliOption("--before-commit-id")]
    public string? BeforeCommitId { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}