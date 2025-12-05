using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mediapackage", "create-harvest-job")]
public record AwsMediapackageCreateHarvestJobOptions(
[property: CliOption("--end-time")] string EndTime,
[property: CliOption("--id")] string Id,
[property: CliOption("--origin-endpoint-id")] string OriginEndpointId,
[property: CliOption("--s3-destination")] string S3Destination,
[property: CliOption("--start-time")] string StartTime
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}