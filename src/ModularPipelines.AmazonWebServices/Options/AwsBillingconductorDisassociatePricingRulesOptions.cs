using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("billingconductor", "disassociate-pricing-rules")]
public record AwsBillingconductorDisassociatePricingRulesOptions(
[property: CliOption("--arn")] string Arn,
[property: CliOption("--pricing-rule-arns")] string[] PricingRuleArns
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}