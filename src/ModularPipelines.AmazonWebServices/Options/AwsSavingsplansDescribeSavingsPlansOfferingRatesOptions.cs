using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("savingsplans", "describe-savings-plans-offering-rates")]
public record AwsSavingsplansDescribeSavingsPlansOfferingRatesOptions : AwsOptions
{
    [CliOption("--savings-plan-offering-ids")]
    public string[]? SavingsPlanOfferingIds { get; set; }

    [CliOption("--savings-plan-payment-options")]
    public string[]? SavingsPlanPaymentOptions { get; set; }

    [CliOption("--savings-plan-types")]
    public string[]? SavingsPlanTypes { get; set; }

    [CliOption("--products")]
    public string[]? Products { get; set; }

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