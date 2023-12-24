using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("glue", "get-ml-task-run")]
public record AwsGlueGetMlTaskRunOptions(
[property: CommandSwitch("--transform-id")] string TransformId,
[property: CommandSwitch("--task-run-id")] string TaskRunId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}