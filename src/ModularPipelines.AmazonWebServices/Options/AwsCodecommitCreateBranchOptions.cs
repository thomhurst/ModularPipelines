using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codecommit", "create-branch")]
public record AwsCodecommitCreateBranchOptions(
[property: CommandSwitch("--repository-name")] string RepositoryName,
[property: CommandSwitch("--branch-name")] string BranchName,
[property: CommandSwitch("--commit-id")] string CommitId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}