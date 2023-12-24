using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codecatalyst", "get-workflow-run")]
public record AwsCodecatalystGetWorkflowRunOptions(
[property: CommandSwitch("--space-name")] string SpaceName,
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--project-name")] string ProjectName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}