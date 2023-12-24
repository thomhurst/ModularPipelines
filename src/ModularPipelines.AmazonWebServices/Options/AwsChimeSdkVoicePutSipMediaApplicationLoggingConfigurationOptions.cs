using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-voice", "put-sip-media-application-logging-configuration")]
public record AwsChimeSdkVoicePutSipMediaApplicationLoggingConfigurationOptions(
[property: CommandSwitch("--sip-media-application-id")] string SipMediaApplicationId
) : AwsOptions
{
    [CommandSwitch("--sip-media-application-logging-configuration")]
    public string? SipMediaApplicationLoggingConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}