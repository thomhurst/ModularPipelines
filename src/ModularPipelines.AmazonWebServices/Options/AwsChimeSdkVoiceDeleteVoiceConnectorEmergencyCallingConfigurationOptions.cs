using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-voice", "delete-voice-connector-emergency-calling-configuration")]
public record AwsChimeSdkVoiceDeleteVoiceConnectorEmergencyCallingConfigurationOptions(
[property: CliOption("--voice-connector-id")] string VoiceConnectorId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}