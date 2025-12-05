using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iottwinmaker", "update-pricing-plan")]
public record AwsIottwinmakerUpdatePricingPlanOptions(
[property: CliOption("--pricing-mode")] string PricingMode
) : AwsOptions
{
    [CliOption("--bundle-names")]
    public string[]? BundleNames { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}