using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codecommit", "describe-merge-conflicts")]
public record AwsCodecommitDescribeMergeConflictsOptions(
[property: CommandSwitch("--repository-name")] string RepositoryName,
[property: CommandSwitch("--destination-commit-specifier")] string DestinationCommitSpecifier,
[property: CommandSwitch("--source-commit-specifier")] string SourceCommitSpecifier,
[property: CommandSwitch("--merge-option")] string MergeOption,
[property: CommandSwitch("--file-path")] string FilePath
) : AwsOptions
{
    [CommandSwitch("--max-merge-hunks")]
    public int? MaxMergeHunks { get; set; }

    [CommandSwitch("--conflict-detail-level")]
    public string? ConflictDetailLevel { get; set; }

    [CommandSwitch("--conflict-resolution-strategy")]
    public string? ConflictResolutionStrategy { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}