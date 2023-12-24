using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codecommit", "batch-describe-merge-conflicts")]
public record AwsCodecommitBatchDescribeMergeConflictsOptions(
[property: CommandSwitch("--repository-name")] string RepositoryName,
[property: CommandSwitch("--destination-commit-specifier")] string DestinationCommitSpecifier,
[property: CommandSwitch("--source-commit-specifier")] string SourceCommitSpecifier,
[property: CommandSwitch("--merge-option")] string MergeOption
) : AwsOptions
{
    [CommandSwitch("--max-merge-hunks")]
    public int? MaxMergeHunks { get; set; }

    [CommandSwitch("--max-conflict-files")]
    public int? MaxConflictFiles { get; set; }

    [CommandSwitch("--file-paths")]
    public string[]? FilePaths { get; set; }

    [CommandSwitch("--conflict-detail-level")]
    public string? ConflictDetailLevel { get; set; }

    [CommandSwitch("--conflict-resolution-strategy")]
    public string? ConflictResolutionStrategy { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}