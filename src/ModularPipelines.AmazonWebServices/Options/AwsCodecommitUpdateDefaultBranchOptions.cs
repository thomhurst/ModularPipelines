using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codecommit", "update-default-branch")]
public record AwsCodecommitUpdateDefaultBranchOptions(
[property: CommandSwitch("--repository-name")] string RepositoryName,
[property: CommandSwitch("--default-branch-name")] string DefaultBranchName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}