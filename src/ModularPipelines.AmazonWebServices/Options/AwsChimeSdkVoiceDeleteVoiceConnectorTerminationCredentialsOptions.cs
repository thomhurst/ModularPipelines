using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-voice", "delete-voice-connector-termination-credentials")]
public record AwsChimeSdkVoiceDeleteVoiceConnectorTerminationCredentialsOptions(
[property: CommandSwitch("--voice-connector-id")] string VoiceConnectorId,
[property: CommandSwitch("--usernames")] string[] Usernames
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}