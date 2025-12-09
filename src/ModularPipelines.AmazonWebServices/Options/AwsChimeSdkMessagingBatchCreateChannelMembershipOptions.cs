using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-messaging", "batch-create-channel-membership")]
public record AwsChimeSdkMessagingBatchCreateChannelMembershipOptions(
[property: CliOption("--channel-arn")] string ChannelArn,
[property: CliOption("--member-arns")] string[] MemberArns,
[property: CliOption("--chime-bearer")] string ChimeBearer
) : AwsOptions
{
    [CliOption("--type")]
    public string? Type { get; set; }

    [CliOption("--sub-channel-id")]
    public string? SubChannelId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}