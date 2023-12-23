using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("billingconductor", "list-pricing-rules-associated-to-pricing-plan")]
public record AwsBillingconductorListPricingRulesAssociatedToPricingPlanOptions(
[property: CommandSwitch("--pricing-plan-arn")] string PricingPlanArn
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