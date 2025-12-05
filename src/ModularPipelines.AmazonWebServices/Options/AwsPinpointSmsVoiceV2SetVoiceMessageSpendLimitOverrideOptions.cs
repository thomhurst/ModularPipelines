using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint-sms-voice-v2", "set-voice-message-spend-limit-override")]
public record AwsPinpointSmsVoiceV2SetVoiceMessageSpendLimitOverrideOptions(
[property: CliOption("--monthly-limit")] long MonthlyLimit
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}