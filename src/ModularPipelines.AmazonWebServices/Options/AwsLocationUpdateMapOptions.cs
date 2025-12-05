using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("location", "update-map")]
public record AwsLocationUpdateMapOptions(
[property: CliOption("--map-name")] string MapName
) : AwsOptions
{
    [CliOption("--configuration-update")]
    public string? ConfigurationUpdate { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--pricing-plan")]
    public string? PricingPlan { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}