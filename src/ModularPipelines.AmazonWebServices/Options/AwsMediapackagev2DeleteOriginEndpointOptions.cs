using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mediapackagev2", "delete-origin-endpoint")]
public record AwsMediapackagev2DeleteOriginEndpointOptions(
[property: CliOption("--channel-group-name")] string ChannelGroupName,
[property: CliOption("--channel-name")] string ChannelName,
[property: CliOption("--origin-endpoint-name")] string OriginEndpointName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}