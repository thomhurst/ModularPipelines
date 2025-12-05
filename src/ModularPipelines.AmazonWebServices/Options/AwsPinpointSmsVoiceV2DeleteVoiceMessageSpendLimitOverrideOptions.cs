using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint-sms-voice-v2", "delete-voice-message-spend-limit-override")]
public record AwsPinpointSmsVoiceV2DeleteVoiceMessageSpendLimitOverrideOptions : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}