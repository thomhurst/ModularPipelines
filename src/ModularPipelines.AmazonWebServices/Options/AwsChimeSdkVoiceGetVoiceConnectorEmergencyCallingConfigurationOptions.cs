using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-voice", "get-voice-connector-emergency-calling-configuration")]
public record AwsChimeSdkVoiceGetVoiceConnectorEmergencyCallingConfigurationOptions(
[property: CommandSwitch("--voice-connector-id")] string VoiceConnectorId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}