using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("glue", "start-export-labels-task-run")]
public record AwsGlueStartExportLabelsTaskRunOptions(
[property: CommandSwitch("--transform-id")] string TransformId,
[property: CommandSwitch("--output-s3-path")] string OutputS3Path
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}