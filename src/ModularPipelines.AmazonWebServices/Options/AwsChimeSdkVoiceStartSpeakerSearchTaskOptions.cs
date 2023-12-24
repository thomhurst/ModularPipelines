using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-voice", "start-speaker-search-task")]
public record AwsChimeSdkVoiceStartSpeakerSearchTaskOptions(
[property: CommandSwitch("--voice-connector-id")] string VoiceConnectorId,
[property: CommandSwitch("--transaction-id")] string TransactionId,
[property: CommandSwitch("--voice-profile-domain-id")] string VoiceProfileDomainId
) : AwsOptions
{
    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--call-leg")]
    public string? CallLeg { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}