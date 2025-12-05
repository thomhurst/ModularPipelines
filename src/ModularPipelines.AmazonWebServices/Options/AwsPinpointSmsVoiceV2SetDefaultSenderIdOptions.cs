using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint-sms-voice-v2", "set-default-sender-id")]
public record AwsPinpointSmsVoiceV2SetDefaultSenderIdOptions(
[property: CliOption("--configuration-set-name")] string ConfigurationSetName,
[property: CliOption("--sender-id")] string SenderId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}