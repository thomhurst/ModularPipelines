using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudtrail", "put-insight-selectors")]
public record AwsCloudtrailPutInsightSelectorsOptions(
[property: CommandSwitch("--insight-selectors")] string[] InsightSelectors
) : AwsOptions
{
    [CommandSwitch("--trail-name")]
    public string? TrailName { get; set; }

    [CommandSwitch("--event-data-store")]
    public string? EventDataStore { get; set; }

    [CommandSwitch("--insights-destination")]
    public string? InsightsDestination { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}