using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("location", "update-tracker")]
public record AwsLocationUpdateTrackerOptions(
[property: CliOption("--tracker-name")] string TrackerName
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--position-filtering")]
    public string? PositionFiltering { get; set; }

    [CliOption("--pricing-plan")]
    public string? PricingPlan { get; set; }

    [CliOption("--pricing-plan-data-source")]
    public string? PricingPlanDataSource { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}