using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint", "delete-segment")]
public record AwsPinpointDeleteSegmentOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--segment-id")] string SegmentId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}