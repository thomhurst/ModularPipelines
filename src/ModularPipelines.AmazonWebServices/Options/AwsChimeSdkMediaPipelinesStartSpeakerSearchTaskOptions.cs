using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-media-pipelines", "start-speaker-search-task")]
public record AwsChimeSdkMediaPipelinesStartSpeakerSearchTaskOptions(
[property: CliOption("--identifier")] string Identifier,
[property: CliOption("--voice-profile-domain-arn")] string VoiceProfileDomainArn
) : AwsOptions
{
    [CliOption("--kinesis-video-stream-source-task-configuration")]
    public string? KinesisVideoStreamSourceTaskConfiguration { get; set; }

    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}