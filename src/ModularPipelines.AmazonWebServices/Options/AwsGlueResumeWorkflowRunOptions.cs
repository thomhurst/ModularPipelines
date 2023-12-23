using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("glue", "resume-workflow-run")]
public record AwsGlueResumeWorkflowRunOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--run-id")] string RunId,
[property: CommandSwitch("--node-ids")] string[] NodeIds
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}