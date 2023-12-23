using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("savingsplans", "describe-savings-plans-offering-rates")]
public record AwsSavingsplansDescribeSavingsPlansOfferingRatesOptions : AwsOptions
{
    [CommandSwitch("--savings-plan-offering-ids")]
    public string[]? SavingsPlanOfferingIds { get; set; }

    [CommandSwitch("--savings-plan-payment-options")]
    public string[]? SavingsPlanPaymentOptions { get; set; }

    [CommandSwitch("--savings-plan-types")]
    public string[]? SavingsPlanTypes { get; set; }

    [CommandSwitch("--products")]
    public string[]? Products { get; set; }

    [CommandSwitch("--service-codes")]
    public string[]? ServiceCodes { get; set; }

    [CommandSwitch("--usage-types")]
    public string[]? UsageTypes { get; set; }

    [CommandSwitch("--operations")]
    public string[]? Operations { get; set; }

    [CommandSwitch("--filters")]
    public string[]? Filters { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}