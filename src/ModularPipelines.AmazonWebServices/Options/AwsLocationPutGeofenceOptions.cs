using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("location", "put-geofence")]
public record AwsLocationPutGeofenceOptions(
[property: CliOption("--collection-name")] string CollectionName,
[property: CliOption("--geofence-id")] string GeofenceId,
[property: CliOption("--geometry")] string Geometry
) : AwsOptions
{
    [CliOption("--geofence-properties")]
    public IEnumerable<KeyValue>? GeofenceProperties { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}