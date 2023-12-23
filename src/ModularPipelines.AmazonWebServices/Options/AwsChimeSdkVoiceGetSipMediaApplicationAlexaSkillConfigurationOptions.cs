using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-voice", "get-sip-media-application-alexa-skill-configuration")]
public record AwsChimeSdkVoiceGetSipMediaApplicationAlexaSkillConfigurationOptions(
[property: CommandSwitch("--sip-media-application-id")] string SipMediaApplicationId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}