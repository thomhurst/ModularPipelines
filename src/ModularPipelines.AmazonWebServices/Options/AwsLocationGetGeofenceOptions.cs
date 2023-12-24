using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("location", "get-geofence")]
public record AwsLocationGetGeofenceOptions(
[property: CommandSwitch("--collection-name")] string CollectionName,
[property: CommandSwitch("--geofence-id")] string GeofenceId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}