using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-media-pipelines", "create-media-stream-pipeline")]
public record AwsChimeSdkMediaPipelinesCreateMediaStreamPipelineOptions(
[property: CliOption("--sources")] string[] Sources,
[property: CliOption("--sinks")] string[] Sinks
) : AwsOptions
{
    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}