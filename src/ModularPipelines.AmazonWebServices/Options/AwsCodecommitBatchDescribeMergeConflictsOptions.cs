using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codecommit", "batch-describe-merge-conflicts")]
public record AwsCodecommitBatchDescribeMergeConflictsOptions(
[property: CliOption("--repository-name")] string RepositoryName,
[property: CliOption("--destination-commit-specifier")] string DestinationCommitSpecifier,
[property: CliOption("--source-commit-specifier")] string SourceCommitSpecifier,
[property: CliOption("--merge-option")] string MergeOption
) : AwsOptions
{
    [CliOption("--max-merge-hunks")]
    public int? MaxMergeHunks { get; set; }

    [CliOption("--max-conflict-files")]
    public int? MaxConflictFiles { get; set; }

    [CliOption("--file-paths")]
    public string[]? FilePaths { get; set; }

    [CliOption("--conflict-detail-level")]
    public string? ConflictDetailLevel { get; set; }

    [CliOption("--conflict-resolution-strategy")]
    public string? ConflictResolutionStrategy { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}