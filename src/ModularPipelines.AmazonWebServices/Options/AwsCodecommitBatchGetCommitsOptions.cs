using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codecommit", "batch-get-commits")]
public record AwsCodecommitBatchGetCommitsOptions(
[property: CliOption("--commit-ids")] string[] CommitIds,
[property: CliOption("--repository-name")] string RepositoryName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}