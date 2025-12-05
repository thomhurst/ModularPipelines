using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-voice", "put-voice-connector-termination")]
public record AwsChimeSdkVoicePutVoiceConnectorTerminationOptions(
[property: CliOption("--voice-connector-id")] string VoiceConnectorId,
[property: CliOption("--termination")] string Termination
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}