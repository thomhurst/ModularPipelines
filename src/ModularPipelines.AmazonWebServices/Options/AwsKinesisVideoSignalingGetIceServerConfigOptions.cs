using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kinesis-video-signaling", "get-ice-server-config")]
public record AwsKinesisVideoSignalingGetIceServerConfigOptions(
[property: CliOption("--channel-arn")] string ChannelArn
) : AwsOptions
{
    [CliOption("--client-id")]
    public string? ClientId { get; set; }

    [CliOption("--service")]
    public string? Service { get; set; }

    [CliOption("--username")]
    public string? Username { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}