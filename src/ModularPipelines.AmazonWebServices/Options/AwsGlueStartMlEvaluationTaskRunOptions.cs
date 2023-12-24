using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("glue", "start-ml-evaluation-task-run")]
public record AwsGlueStartMlEvaluationTaskRunOptions(
[property: CommandSwitch("--transform-id")] string TransformId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}