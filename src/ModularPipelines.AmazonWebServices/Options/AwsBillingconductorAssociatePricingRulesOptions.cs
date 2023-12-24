using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("billingconductor", "associate-pricing-rules")]
public record AwsBillingconductorAssociatePricingRulesOptions(
[property: CommandSwitch("--arn")] string Arn,
[property: CommandSwitch("--pricing-rule-arns")] string[] PricingRuleArns
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}