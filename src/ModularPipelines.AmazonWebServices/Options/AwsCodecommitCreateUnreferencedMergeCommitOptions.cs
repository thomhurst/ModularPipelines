using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codecommit", "create-unreferenced-merge-commit")]
public record AwsCodecommitCreateUnreferencedMergeCommitOptions(
[property: CommandSwitch("--repository-name")] string RepositoryName,
[property: CommandSwitch("--source-commit-specifier")] string SourceCommitSpecifier,
[property: CommandSwitch("--destination-commit-specifier")] string DestinationCommitSpecifier,
[property: CommandSwitch("--merge-option")] string MergeOption
) : AwsOptions
{
    [CommandSwitch("--conflict-detail-level")]
    public string? ConflictDetailLevel { get; set; }

    [CommandSwitch("--conflict-resolution-strategy")]
    public string? ConflictResolutionStrategy { get; set; }

    [CommandSwitch("--author-name")]
    public string? AuthorName { get; set; }

    [CommandSwitch("--email")]
    public string? Email { get; set; }

    [CommandSwitch("--commit-message")]
    public string? CommitMessage { get; set; }

    [CommandSwitch("--conflict-resolution")]
    public string? ConflictResolution { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}