using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudtrail", "put-event-selectors")]
public record AwsCloudtrailPutEventSelectorsOptions(
[property: CliOption("--trail-name")] string TrailName
) : AwsOptions
{
    [CliOption("--event-selectors")]
    public string[]? EventSelectors { get; set; }

    [CliOption("--advanced-event-selectors")]
    public string[]? AdvancedEventSelectors { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}