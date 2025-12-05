using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("location", "update-place-index")]
public record AwsLocationUpdatePlaceIndexOptions(
[property: CliOption("--index-name")] string IndexName
) : AwsOptions
{
    [CliOption("--data-source-configuration")]
    public string? DataSourceConfiguration { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--pricing-plan")]
    public string? PricingPlan { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}