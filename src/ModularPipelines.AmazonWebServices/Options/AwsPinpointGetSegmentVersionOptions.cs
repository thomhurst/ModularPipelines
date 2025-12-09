using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint", "get-segment-version")]
public record AwsPinpointGetSegmentVersionOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--segment-id")] string SegmentId,
[property: CliOption("--segment-version")] string SegmentVersion
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}