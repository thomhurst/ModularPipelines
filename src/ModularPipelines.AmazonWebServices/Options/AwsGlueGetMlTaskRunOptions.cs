using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glue", "get-ml-task-run")]
public record AwsGlueGetMlTaskRunOptions(
[property: CliOption("--transform-id")] string TransformId,
[property: CliOption("--task-run-id")] string TaskRunId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}