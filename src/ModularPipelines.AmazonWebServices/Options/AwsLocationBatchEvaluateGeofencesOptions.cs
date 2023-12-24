using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("location", "batch-evaluate-geofences")]
public record AwsLocationBatchEvaluateGeofencesOptions(
[property: CommandSwitch("--collection-name")] string CollectionName,
[property: CommandSwitch("--device-position-updates")] string[] DevicePositionUpdates
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}