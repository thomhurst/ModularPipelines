using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codecatalyst", "list-source-repository-branches")]
public record AwsCodecatalystListSourceRepositoryBranchesOptions(
[property: CommandSwitch("--space-name")] string SpaceName,
[property: CommandSwitch("--project-name")] string ProjectName,
[property: CommandSwitch("--source-repository-name")] string SourceRepositoryName
) : AwsOptions
{
    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}