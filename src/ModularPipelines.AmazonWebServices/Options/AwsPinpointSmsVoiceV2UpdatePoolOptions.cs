using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint-sms-voice-v2", "update-pool")]
public record AwsPinpointSmsVoiceV2UpdatePoolOptions(
[property: CommandSwitch("--pool-id")] string PoolId
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