using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codecommit", "get-merge-options")]
public record AwsCodecommitGetMergeOptionsOptions(
[property: CommandSwitch("--repository-name")] string RepositoryName,
[property: CommandSwitch("--source-commit-specifier")] string SourceCommitSpecifier,
[property: CommandSwitch("--destination-commit-specifier")] string DestinationCommitSpecifier
) : AwsOptions
{
    [CommandSwitch("--conflict-detail-level")]
    public string? ConflictDetailLevel { get; set; }

    [CommandSwitch("--conflict-resolution-strategy")]
    public string? ConflictResolutionStrategy { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}