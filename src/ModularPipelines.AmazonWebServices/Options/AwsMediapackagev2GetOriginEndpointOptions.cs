using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mediapackagev2", "get-origin-endpoint")]
public record AwsMediapackagev2GetOriginEndpointOptions(
[property: CliOption("--channel-group-name")] string ChannelGroupName,
[property: CliOption("--channel-name")] string ChannelName,
[property: CliOption("--origin-endpoint-name")] string OriginEndpointName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}