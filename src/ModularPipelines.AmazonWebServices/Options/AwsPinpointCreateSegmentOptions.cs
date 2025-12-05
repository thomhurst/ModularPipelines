using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint", "create-segment")]
public record AwsPinpointCreateSegmentOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--write-segment-request")] string WriteSegmentRequest
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}