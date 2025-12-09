using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-messaging", "describe-channel-flow")]
public record AwsChimeSdkMessagingDescribeChannelFlowOptions(
[property: CliOption("--channel-flow-arn")] string ChannelFlowArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}