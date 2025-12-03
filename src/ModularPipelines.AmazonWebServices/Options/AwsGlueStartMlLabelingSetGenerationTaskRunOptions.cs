using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glue", "start-ml-labeling-set-generation-task-run")]
public record AwsGlueStartMlLabelingSetGenerationTaskRunOptions(
[property: CliOption("--transform-id")] string TransformId,
[property: CliOption("--output-s3-path")] string OutputS3Path
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}