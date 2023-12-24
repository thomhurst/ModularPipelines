using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("glue", "cancel-ml-task-run")]
public record AwsGlueCancelMlTaskRunOptions(
[property: CommandSwitch("--transform-id")] string TransformId,
[property: CommandSwitch("--task-run-id")] string TaskRunId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}