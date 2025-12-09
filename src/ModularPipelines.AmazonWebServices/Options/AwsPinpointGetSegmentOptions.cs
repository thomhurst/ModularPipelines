using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint", "get-segment")]
public record AwsPinpointGetSegmentOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--segment-id")] string SegmentId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}