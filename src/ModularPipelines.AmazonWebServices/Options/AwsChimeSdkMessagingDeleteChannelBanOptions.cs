using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-messaging", "delete-channel-ban")]
public record AwsChimeSdkMessagingDeleteChannelBanOptions(
[property: CliOption("--channel-arn")] string ChannelArn,
[property: CliOption("--member-arn")] string MemberArn,
[property: CliOption("--chime-bearer")] string ChimeBearer
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}