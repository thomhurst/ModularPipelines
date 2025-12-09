using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("savingsplans", "describe-savings-plans-offerings")]
public record AwsSavingsplansDescribeSavingsPlansOfferingsOptions : AwsOptions
{
    [CliOption("--offering-ids")]
    public string[]? OfferingIds { get; set; }

    [CliOption("--payment-options")]
    public string[]? PaymentOptions { get; set; }

    [CliOption("--product-type")]
    public string? ProductType { get; set; }

    [CliOption("--plan-types")]
    public string[]? PlanTypes { get; set; }

    [CliOption("--durations")]
    public string[]? Durations { get; set; }

    [CliOption("--currencies")]
    public string[]? Currencies { get; set; }

    [CliOption("--descriptions")]
    public string[]? Descriptions { get; set; }

    [CliOption("--service-codes")]
    public string[]? ServiceCodes { get; set; }

    [CliOption("--usage-types")]
    public string[]? UsageTypes { get; set; }

    [CliOption("--operations")]
    public string[]? Operations { get; set; }

    [CliOption("--filters")]
    public string[]? Filters { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}