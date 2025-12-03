using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("billingconductor", "associate-pricing-rules")]
public record AwsBillingconductorAssociatePricingRulesOptions(
[property: CliOption("--arn")] string Arn,
[property: CliOption("--pricing-rule-arns")] string[] PricingRuleArns
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}