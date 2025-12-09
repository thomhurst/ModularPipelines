using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-messaging", "describe-channel-moderated-by-app-instance-user")]
public record AwsChimeSdkMessagingDescribeChannelModeratedByAppInstanceUserOptions(
[property: CliOption("--channel-arn")] string ChannelArn,
[property: CliOption("--app-instance-user-arn")] string AppInstanceUserArn,
[property: CliOption("--chime-bearer")] string ChimeBearer
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}