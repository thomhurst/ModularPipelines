using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint-sms-voice-v2", "set-text-message-spend-limit-override")]
public record AwsPinpointSmsVoiceV2SetTextMessageSpendLimitOverrideOptions(
[property: CommandSwitch("--monthly-limit")] long MonthlyLimit
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}