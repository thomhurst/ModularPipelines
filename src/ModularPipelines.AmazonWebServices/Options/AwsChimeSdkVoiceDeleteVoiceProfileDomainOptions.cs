using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-voice", "delete-voice-profile-domain")]
public record AwsChimeSdkVoiceDeleteVoiceProfileDomainOptions(
[property: CommandSwitch("--voice-profile-domain-id")] string VoiceProfileDomainId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}