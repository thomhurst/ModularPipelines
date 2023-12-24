using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("location", "batch-delete-geofence")]
public record AwsLocationBatchDeleteGeofenceOptions(
[property: CommandSwitch("--collection-name")] string CollectionName,
[property: CommandSwitch("--geofence-ids")] string[] GeofenceIds
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}