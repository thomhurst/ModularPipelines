using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-voice", "put-voice-connector-termination-credentials")]
public record AwsChimeSdkVoicePutVoiceConnectorTerminationCredentialsOptions(
[property: CommandSwitch("--voice-connector-id")] string VoiceConnectorId
) : AwsOptions
{
    [CommandSwitch("--credentials")]
    public string[]? Credentials { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}