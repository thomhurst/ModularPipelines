using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-voice", "put-sip-media-application-alexa-skill-configuration")]
public record AwsChimeSdkVoicePutSipMediaApplicationAlexaSkillConfigurationOptions(
[property: CommandSwitch("--sip-media-application-id")] string SipMediaApplicationId
) : AwsOptions
{
    [CommandSwitch("--sip-media-application-alexa-skill-configuration")]
    public string? SipMediaApplicationAlexaSkillConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}