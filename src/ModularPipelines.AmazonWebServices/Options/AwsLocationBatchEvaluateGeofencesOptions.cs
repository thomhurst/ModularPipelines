using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("location", "batch-evaluate-geofences")]
public record AwsLocationBatchEvaluateGeofencesOptions(
[property: CliOption("--collection-name")] string CollectionName,
[property: CliOption("--device-position-updates")] string[] DevicePositionUpdates
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}