using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-voice", "put-voice-connector-emergency-calling-configuration")]
public record AwsChimeSdkVoicePutVoiceConnectorEmergencyCallingConfigurationOptions(
[property: CliOption("--voice-connector-id")] string VoiceConnectorId,
[property: CliOption("--emergency-calling-configuration")] string EmergencyCallingConfiguration
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}