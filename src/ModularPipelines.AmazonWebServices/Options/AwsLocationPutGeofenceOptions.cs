using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("location", "put-geofence")]
public record AwsLocationPutGeofenceOptions(
[property: CommandSwitch("--collection-name")] string CollectionName,
[property: CommandSwitch("--geofence-id")] string GeofenceId,
[property: CommandSwitch("--geometry")] string Geometry
) : AwsOptions
{
    [CommandSwitch("--geofence-properties")]
    public IEnumerable<KeyValue>? GeofenceProperties { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}