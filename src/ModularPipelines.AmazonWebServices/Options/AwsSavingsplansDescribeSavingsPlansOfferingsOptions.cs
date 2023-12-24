using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("savingsplans", "describe-savings-plans-offerings")]
public record AwsSavingsplansDescribeSavingsPlansOfferingsOptions : AwsOptions
{
    [CommandSwitch("--offering-ids")]
    public string[]? OfferingIds { get; set; }

    [CommandSwitch("--payment-options")]
    public string[]? PaymentOptions { get; set; }

    [CommandSwitch("--product-type")]
    public string? ProductType { get; set; }

    [CommandSwitch("--plan-types")]
    public string[]? PlanTypes { get; set; }

    [CommandSwitch("--durations")]
    public string[]? Durations { get; set; }

    [CommandSwitch("--currencies")]
    public string[]? Currencies { get; set; }

    [CommandSwitch("--descriptions")]
    public string[]? Descriptions { get; set; }

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