using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-media-pipelines", "start-speaker-search-task")]
public record AwsChimeSdkMediaPipelinesStartSpeakerSearchTaskOptions(
[property: CommandSwitch("--identifier")] string Identifier,
[property: CommandSwitch("--voice-profile-domain-arn")] string VoiceProfileDomainArn
) : AwsOptions
{
    [CommandSwitch("--kinesis-video-stream-source-task-configuration")]
    public string? KinesisVideoStreamSourceTaskConfiguration { get; set; }

    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}