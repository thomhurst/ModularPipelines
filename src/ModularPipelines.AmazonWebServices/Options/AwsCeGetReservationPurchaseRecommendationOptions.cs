using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ce", "get-reservation-purchase-recommendation")]
public record AwsCeGetReservationPurchaseRecommendationOptions(
[property: CliOption("--service")] string Service
) : AwsOptions
{
    [CliOption("--account-id")]
    public string? AccountId { get; set; }

    [CliOption("--filter")]
    public string? Filter { get; set; }

    [CliOption("--account-scope")]
    public string? AccountScope { get; set; }

    [CliOption("--lookback-period-in-days")]
    public string? LookbackPeriodInDays { get; set; }

    [CliOption("--term-in-years")]
    public string? TermInYears { get; set; }

    [CliOption("--payment-option")]
    public string? PaymentOption { get; set; }

    [CliOption("--service-specification")]
    public string? ServiceSpecification { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--next-page-token")]
    public string? NextPageToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}