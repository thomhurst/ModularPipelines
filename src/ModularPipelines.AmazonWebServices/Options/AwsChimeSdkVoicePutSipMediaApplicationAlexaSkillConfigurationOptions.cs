using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-voice", "put-sip-media-application-alexa-skill-configuration")]
public record AwsChimeSdkVoicePutSipMediaApplicationAlexaSkillConfigurationOptions(
[property: CliOption("--sip-media-application-id")] string SipMediaApplicationId
) : AwsOptions
{
    [CliOption("--sip-media-application-alexa-skill-configuration")]
    public string? SipMediaApplicationAlexaSkillConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}