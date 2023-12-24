using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("location", "update-geofence-collection")]
public record AwsLocationUpdateGeofenceCollectionOptions(
[property: CommandSwitch("--collection-name")] string CollectionName
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--pricing-plan")]
    public string? PricingPlan { get; set; }

    [CommandSwitch("--pricing-plan-data-source")]
    public string? PricingPlanDataSource { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}