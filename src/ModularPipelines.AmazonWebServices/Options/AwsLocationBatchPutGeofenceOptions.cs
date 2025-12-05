using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("location", "batch-put-geofence")]
public record AwsLocationBatchPutGeofenceOptions(
[property: CliOption("--collection-name")] string CollectionName,
[property: CliOption("--entries")] string[] Entries
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}