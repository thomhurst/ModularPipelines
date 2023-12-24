using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codecommit", "get-branch")]
public record AwsCodecommitGetBranchOptions : AwsOptions
{
    [CommandSwitch("--repository-name")]
    public string? RepositoryName { get; set; }

    [CommandSwitch("--branch-name")]
    public string? BranchName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}