using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint-sms-voice-v2", "set-voice-message-spend-limit-override")]
public record AwsPinpointSmsVoiceV2SetVoiceMessageSpendLimitOverrideOptions(
[property: CommandSwitch("--monthly-limit")] long MonthlyLimit
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}