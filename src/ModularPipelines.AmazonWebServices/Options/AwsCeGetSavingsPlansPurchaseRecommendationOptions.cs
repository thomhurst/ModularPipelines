using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ce", "get-savings-plans-purchase-recommendation")]
public record AwsCeGetSavingsPlansPurchaseRecommendationOptions(
[property: CliOption("--savings-plans-type")] string SavingsPlansType,
[property: CliOption("--term-in-years")] string TermInYears,
[property: CliOption("--payment-option")] string PaymentOption,
[property: CliOption("--lookback-period-in-days")] string LookbackPeriodInDays
) : AwsOptions
{
    [CliOption("--account-scope")]
    public string? AccountScope { get; set; }

    [CliOption("--next-page-token")]
    public string? NextPageToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--filter")]
    public string? Filter { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}