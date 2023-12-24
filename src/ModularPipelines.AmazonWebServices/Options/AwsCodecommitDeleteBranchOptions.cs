using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codecommit", "delete-branch")]
public record AwsCodecommitDeleteBranchOptions(
[property: CommandSwitch("--repository-name")] string RepositoryName,
[property: CommandSwitch("--branch-name")] string BranchName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}