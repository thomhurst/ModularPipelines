using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codecommit", "get-merge-commit")]
public record AwsCodecommitGetMergeCommitOptions(
[property: CliOption("--repository-name")] string RepositoryName,
[property: CliOption("--source-commit-specifier")] string SourceCommitSpecifier,
[property: CliOption("--destination-commit-specifier")] string DestinationCommitSpecifier
) : AwsOptions
{
    [CliOption("--conflict-detail-level")]
    public string? ConflictDetailLevel { get; set; }

    [CliOption("--conflict-resolution-strategy")]
    public string? ConflictResolutionStrategy { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}