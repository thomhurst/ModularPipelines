using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-messaging", "associate-channel-flow")]
public record AwsChimeSdkMessagingAssociateChannelFlowOptions(
[property: CliOption("--channel-arn")] string ChannelArn,
[property: CliOption("--channel-flow-arn")] string ChannelFlowArn,
[property: CliOption("--chime-bearer")] string ChimeBearer
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}