using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint-sms-voice-v2", "update-phone-number")]
public record AwsPinpointSmsVoiceV2UpdatePhoneNumberOptions(
[property: CliOption("--phone-number-id")] string PhoneNumberId
) : AwsOptions
{
    [CliOption("--two-way-channel-arn")]
    public string? TwoWayChannelArn { get; set; }

    [CliOption("--two-way-channel-role")]
    public string? TwoWayChannelRole { get; set; }

    [CliOption("--opt-out-list-name")]
    public string? OptOutListName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}