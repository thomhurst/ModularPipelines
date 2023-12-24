using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("location", "update-tracker")]
public record AwsLocationUpdateTrackerOptions(
[property: CommandSwitch("--tracker-name")] string TrackerName
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--position-filtering")]
    public string? PositionFiltering { get; set; }

    [CommandSwitch("--pricing-plan")]
    public string? PricingPlan { get; set; }

    [CommandSwitch("--pricing-plan-data-source")]
    public string? PricingPlanDataSource { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}