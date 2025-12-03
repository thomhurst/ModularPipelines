using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-messaging", "delete-channel-moderator")]
public record AwsChimeSdkMessagingDeleteChannelModeratorOptions(
[property: CliOption("--channel-arn")] string ChannelArn,
[property: CliOption("--channel-moderator-arn")] string ChannelModeratorArn,
[property: CliOption("--chime-bearer")] string ChimeBearer
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}