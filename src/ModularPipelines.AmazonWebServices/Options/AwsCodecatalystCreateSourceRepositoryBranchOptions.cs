using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codecatalyst", "create-source-repository-branch")]
public record AwsCodecatalystCreateSourceRepositoryBranchOptions(
[property: CommandSwitch("--space-name")] string SpaceName,
[property: CommandSwitch("--project-name")] string ProjectName,
[property: CommandSwitch("--source-repository-name")] string SourceRepositoryName,
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--head-commit-id")]
    public string? HeadCommitId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}