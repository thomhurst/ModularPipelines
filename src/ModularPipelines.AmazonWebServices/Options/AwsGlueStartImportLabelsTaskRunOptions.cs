using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glue", "start-import-labels-task-run")]
public record AwsGlueStartImportLabelsTaskRunOptions(
[property: CliOption("--transform-id")] string TransformId,
[property: CliOption("--input-s3-path")] string InputS3Path
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}