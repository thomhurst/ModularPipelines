using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codecommit", "merge-branches-by-fast-forward")]
public record AwsCodecommitMergeBranchesByFastForwardOptions(
[property: CliOption("--repository-name")] string RepositoryName,
[property: CliOption("--source-commit-specifier")] string SourceCommitSpecifier,
[property: CliOption("--destination-commit-specifier")] string DestinationCommitSpecifier
) : AwsOptions
{
    [CliOption("--target-branch")]
    public string? TargetBranch { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}