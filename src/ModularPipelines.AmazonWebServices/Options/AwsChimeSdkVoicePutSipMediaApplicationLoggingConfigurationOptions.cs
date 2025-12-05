using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-voice", "put-sip-media-application-logging-configuration")]
public record AwsChimeSdkVoicePutSipMediaApplicationLoggingConfigurationOptions(
[property: CliOption("--sip-media-application-id")] string SipMediaApplicationId
) : AwsOptions
{
    [CliOption("--sip-media-application-logging-configuration")]
    public string? SipMediaApplicationLoggingConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}