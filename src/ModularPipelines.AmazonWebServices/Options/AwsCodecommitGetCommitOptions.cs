using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codecommit", "get-commit")]
public record AwsCodecommitGetCommitOptions(
[property: CliOption("--repository-name")] string RepositoryName,
[property: CliOption("--commit-id")] string CommitId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}