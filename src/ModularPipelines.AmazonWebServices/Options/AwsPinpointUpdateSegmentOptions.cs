using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint", "update-segment")]
public record AwsPinpointUpdateSegmentOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--segment-id")] string SegmentId,
[property: CliOption("--write-segment-request")] string WriteSegmentRequest
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}