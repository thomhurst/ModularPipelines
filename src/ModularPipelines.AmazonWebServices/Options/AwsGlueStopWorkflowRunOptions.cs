using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("glue", "stop-workflow-run")]
public record AwsGlueStopWorkflowRunOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--run-id")] string RunId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}