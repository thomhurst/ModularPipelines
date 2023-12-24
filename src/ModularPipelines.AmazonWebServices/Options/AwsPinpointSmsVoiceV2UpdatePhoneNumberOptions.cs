using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint-sms-voice-v2", "update-phone-number")]
public record AwsPinpointSmsVoiceV2UpdatePhoneNumberOptions(
[property: CommandSwitch("--phone-number-id")] string PhoneNumberId
) : AwsOptions
{
    [CommandSwitch("--two-way-channel-arn")]
    public string? TwoWayChannelArn { get; set; }

    [CommandSwitch("--two-way-channel-role")]
    public string? TwoWayChannelRole { get; set; }

    [CommandSwitch("--opt-out-list-name")]
    public string? OptOutListName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}