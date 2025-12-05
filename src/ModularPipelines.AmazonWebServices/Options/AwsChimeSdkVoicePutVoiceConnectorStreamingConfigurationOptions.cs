using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-voice", "put-voice-connector-streaming-configuration")]
public record AwsChimeSdkVoicePutVoiceConnectorStreamingConfigurationOptions(
[property: CliOption("--voice-connector-id")] string VoiceConnectorId,
[property: CliOption("--streaming-configuration")] string StreamingConfiguration
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}