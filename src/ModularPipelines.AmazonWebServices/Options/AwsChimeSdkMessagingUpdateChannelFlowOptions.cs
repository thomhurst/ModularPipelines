using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-messaging", "update-channel-flow")]
public record AwsChimeSdkMessagingUpdateChannelFlowOptions(
[property: CliOption("--channel-flow-arn")] string ChannelFlowArn,
[property: CliOption("--processors")] string[] Processors,
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}