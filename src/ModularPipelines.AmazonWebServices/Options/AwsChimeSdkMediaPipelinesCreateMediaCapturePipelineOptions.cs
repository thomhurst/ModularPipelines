using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-media-pipelines", "create-media-capture-pipeline")]
public record AwsChimeSdkMediaPipelinesCreateMediaCapturePipelineOptions(
[property: CliOption("--source-type")] string SourceType,
[property: CliOption("--source-arn")] string SourceArn,
[property: CliOption("--sink-type")] string SinkType,
[property: CliOption("--sink-arn")] string SinkArn
) : AwsOptions
{
    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--chime-sdk-meeting-configuration")]
    public string? ChimeSdkMeetingConfiguration { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}