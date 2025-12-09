using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint-sms-voice-v2", "set-default-message-type")]
public record AwsPinpointSmsVoiceV2SetDefaultMessageTypeOptions(
[property: CliOption("--configuration-set-name")] string ConfigurationSetName,
[property: CliOption("--message-type")] string MessageType
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}