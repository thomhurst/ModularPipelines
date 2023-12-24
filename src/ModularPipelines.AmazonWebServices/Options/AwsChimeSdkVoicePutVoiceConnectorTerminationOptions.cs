using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-voice", "put-voice-connector-termination")]
public record AwsChimeSdkVoicePutVoiceConnectorTerminationOptions(
[property: CommandSwitch("--voice-connector-id")] string VoiceConnectorId,
[property: CommandSwitch("--termination")] string Termination
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}