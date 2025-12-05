using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codecommit", "merge-branches-by-squash")]
public record AwsCodecommitMergeBranchesBySquashOptions(
[property: CliOption("--repository-name")] string RepositoryName,
[property: CliOption("--source-commit-specifier")] string SourceCommitSpecifier,
[property: CliOption("--destination-commit-specifier")] string DestinationCommitSpecifier
) : AwsOptions
{
    [CliOption("--target-branch")]
    public string? TargetBranch { get; set; }

    [CliOption("--conflict-detail-level")]
    public string? ConflictDetailLevel { get; set; }

    [CliOption("--conflict-resolution-strategy")]
    public string? ConflictResolutionStrategy { get; set; }

    [CliOption("--author-name")]
    public string? AuthorName { get; set; }

    [CliOption("--email")]
    public string? Email { get; set; }

    [CliOption("--commit-message")]
    public string? CommitMessage { get; set; }

    [CliOption("--conflict-resolution")]
    public string? ConflictResolution { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}