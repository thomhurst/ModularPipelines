using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("osis", "get-pipeline-change-progress")]
public record AwsOsisGetPipelineChangeProgressOptions(
[property: CliOption("--pipeline-name")] string PipelineName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}