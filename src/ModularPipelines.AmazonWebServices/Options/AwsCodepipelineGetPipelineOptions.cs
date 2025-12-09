using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codepipeline", "get-pipeline")]
public record AwsCodepipelineGetPipelineOptions(
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--pipeline-version")]
    public int? PipelineVersion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}