using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codecommit", "get-merge-conflicts")]
public record AwsCodecommitGetMergeConflictsOptions(
[property: CliOption("--repository-name")] string RepositoryName,
[property: CliOption("--destination-commit-specifier")] string DestinationCommitSpecifier,
[property: CliOption("--source-commit-specifier")] string SourceCommitSpecifier,
[property: CliOption("--merge-option")] string MergeOption
) : AwsOptions
{
    [CliOption("--conflict-detail-level")]
    public string? ConflictDetailLevel { get; set; }

    [CliOption("--max-conflict-files")]
    public int? MaxConflictFiles { get; set; }

    [CliOption("--conflict-resolution-strategy")]
    public string? ConflictResolutionStrategy { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}