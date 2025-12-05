using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint-sms-voice-v2", "update-pool")]
public record AwsPinpointSmsVoiceV2UpdatePoolOptions(
[property: CliOption("--pool-id")] string PoolId
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