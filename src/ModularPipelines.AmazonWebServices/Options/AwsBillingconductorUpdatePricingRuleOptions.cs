using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("billingconductor", "update-pricing-rule")]
public record AwsBillingconductorUpdatePricingRuleOptions(
[property: CliOption("--arn")] string Arn
) : AwsOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }

    [CliOption("--modifier-percentage")]
    public double? ModifierPercentage { get; set; }

    [CliOption("--tiering")]
    public string? Tiering { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}