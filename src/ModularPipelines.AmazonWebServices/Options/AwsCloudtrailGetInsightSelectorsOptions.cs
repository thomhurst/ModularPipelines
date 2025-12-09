using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudtrail", "get-insight-selectors")]
public record AwsCloudtrailGetInsightSelectorsOptions : AwsOptions
{
    [CliOption("--trail-name")]
    public string? TrailName { get; set; }

    [CliOption("--event-data-store")]
    public string? EventDataStore { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}