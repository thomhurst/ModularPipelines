using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codecatalyst", "start-workflow-run")]
public record AwsCodecatalystStartWorkflowRunOptions(
[property: CommandSwitch("--space-name")] string SpaceName,
[property: CommandSwitch("--project-name")] string ProjectName,
[property: CommandSwitch("--workflow-id")] string WorkflowId
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}