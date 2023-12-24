using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codecommit", "get-commit")]
public record AwsCodecommitGetCommitOptions(
[property: CommandSwitch("--repository-name")] string RepositoryName,
[property: CommandSwitch("--commit-id")] string CommitId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}