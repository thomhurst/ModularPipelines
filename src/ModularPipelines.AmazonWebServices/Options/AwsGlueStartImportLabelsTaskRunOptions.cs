using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("glue", "start-import-labels-task-run")]
public record AwsGlueStartImportLabelsTaskRunOptions(
[property: CommandSwitch("--transform-id")] string TransformId,
[property: CommandSwitch("--input-s3-path")] string InputS3Path
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}