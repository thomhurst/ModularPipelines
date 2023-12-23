using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codecommit", "merge-branches-by-fast-forward")]
public record AwsCodecommitMergeBranchesByFastForwardOptions(
[property: CommandSwitch("--repository-name")] string RepositoryName,
[property: CommandSwitch("--source-commit-specifier")] string SourceCommitSpecifier,
[property: CommandSwitch("--destination-commit-specifier")] string DestinationCommitSpecifier
) : AwsOptions
{
    [CommandSwitch("--target-branch")]
    public string? TargetBranch { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}