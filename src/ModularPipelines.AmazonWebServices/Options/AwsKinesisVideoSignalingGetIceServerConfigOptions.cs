using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kinesis-video-signaling", "get-ice-server-config")]
public record AwsKinesisVideoSignalingGetIceServerConfigOptions(
[property: CommandSwitch("--channel-arn")] string ChannelArn
) : AwsOptions
{
    [CommandSwitch("--client-id")]
    public string? ClientId { get; set; }

    [CommandSwitch("--service")]
    public string? Service { get; set; }

    [CommandSwitch("--username")]
    public string? Username { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}