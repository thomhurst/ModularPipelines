using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-messaging", "describe-channel")]
public record AwsChimeSdkMessagingDescribeChannelOptions(
[property: CliOption("--channel-arn")] string ChannelArn,
[property: CliOption("--chime-bearer")] string ChimeBearer
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}