using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codecommit", "describe-merge-conflicts")]
public record AwsCodecommitDescribeMergeConflictsOptions(
[property: CliOption("--repository-name")] string RepositoryName,
[property: CliOption("--destination-commit-specifier")] string DestinationCommitSpecifier,
[property: CliOption("--source-commit-specifier")] string SourceCommitSpecifier,
[property: CliOption("--merge-option")] string MergeOption,
[property: CliOption("--file-path")] string FilePath
) : AwsOptions
{
    [CliOption("--max-merge-hunks")]
    public int? MaxMergeHunks { get; set; }

    [CliOption("--conflict-detail-level")]
    public string? ConflictDetailLevel { get; set; }

    [CliOption("--conflict-resolution-strategy")]
    public string? ConflictResolutionStrategy { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}