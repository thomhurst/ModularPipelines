using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("location", "batch-delete-geofence")]
public record AwsLocationBatchDeleteGeofenceOptions(
[property: CliOption("--collection-name")] string CollectionName,
[property: CliOption("--geofence-ids")] string[] GeofenceIds
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}