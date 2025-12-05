using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("location", "update-geofence-collection")]
public record AwsLocationUpdateGeofenceCollectionOptions(
[property: CliOption("--collection-name")] string CollectionName
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--pricing-plan")]
    public string? PricingPlan { get; set; }

    [CliOption("--pricing-plan-data-source")]
    public string? PricingPlanDataSource { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}