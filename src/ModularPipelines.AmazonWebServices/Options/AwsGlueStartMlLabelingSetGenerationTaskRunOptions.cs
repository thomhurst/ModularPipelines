using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("glue", "start-ml-labeling-set-generation-task-run")]
public record AwsGlueStartMlLabelingSetGenerationTaskRunOptions(
[property: CommandSwitch("--transform-id")] string TransformId,
[property: CommandSwitch("--output-s3-path")] string OutputS3Path
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}