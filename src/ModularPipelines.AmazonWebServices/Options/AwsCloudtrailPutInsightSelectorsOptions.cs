using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudtrail", "put-insight-selectors")]
public record AwsCloudtrailPutInsightSelectorsOptions(
[property: CliOption("--insight-selectors")] string[] InsightSelectors
) : AwsOptions
{
    [CliOption("--trail-name")]
    public string? TrailName { get; set; }

    [CliOption("--event-data-store")]
    public string? EventDataStore { get; set; }

    [CliOption("--insights-destination")]
    public string? InsightsDestination { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}