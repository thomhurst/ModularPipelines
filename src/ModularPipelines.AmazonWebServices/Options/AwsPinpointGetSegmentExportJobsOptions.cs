using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint", "get-segment-export-jobs")]
public record AwsPinpointGetSegmentExportJobsOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--segment-id")] string SegmentId
) : AwsOptions
{
    [CliOption("--page-size")]
    public string? PageSize { get; set; }

    [CliOption("--token")]
    public string? Token { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}