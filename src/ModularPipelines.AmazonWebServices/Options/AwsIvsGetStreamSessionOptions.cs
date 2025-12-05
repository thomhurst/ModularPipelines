using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ivs", "get-stream-session")]
public record AwsIvsGetStreamSessionOptions(
[property: CliOption("--channel-arn")] string ChannelArn
) : AwsOptions
{
    [CliOption("--stream-id")]
    public string? StreamId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}