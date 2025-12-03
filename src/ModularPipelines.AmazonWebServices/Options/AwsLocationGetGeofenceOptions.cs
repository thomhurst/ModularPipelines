using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("location", "get-geofence")]
public record AwsLocationGetGeofenceOptions(
[property: CliOption("--collection-name")] string CollectionName,
[property: CliOption("--geofence-id")] string GeofenceId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}