using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("billingconductor", "list-pricing-plans-associated-with-pricing-rule")]
public record AwsBillingconductorListPricingPlansAssociatedWithPricingRuleOptions(
[property: CommandSwitch("--pricing-rule-arn")] string PricingRuleArn
) : AwsOptions
{
    [CommandSwitch("--billing-period")]
    public string? BillingPeriod { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}