using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("billingconductor", "list-pricing-plans-associated-with-pricing-rule")]
public record AwsBillingconductorListPricingPlansAssociatedWithPricingRuleOptions(
[property: CliOption("--pricing-rule-arn")] string PricingRuleArn
) : AwsOptions
{
    [CliOption("--billing-period")]
    public string? BillingPeriod { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}