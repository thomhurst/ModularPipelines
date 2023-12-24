using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-media-pipelines", "create-media-capture-pipeline")]
public record AwsChimeSdkMediaPipelinesCreateMediaCapturePipelineOptions(
[property: CommandSwitch("--source-type")] string SourceType,
[property: CommandSwitch("--source-arn")] string SourceArn,
[property: CommandSwitch("--sink-type")] string SinkType,
[property: CommandSwitch("--sink-arn")] string SinkArn
) : AwsOptions
{
    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--chime-sdk-meeting-configuration")]
    public string? ChimeSdkMeetingConfiguration { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}