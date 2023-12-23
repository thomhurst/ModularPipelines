using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ce", "get-reservation-purchase-recommendation")]
public record AwsCeGetReservationPurchaseRecommendationOptions(
[property: CommandSwitch("--service")] string Service
) : AwsOptions
{
    [CommandSwitch("--account-id")]
    public string? AccountId { get; set; }

    [CommandSwitch("--filter")]
    public string? Filter { get; set; }

    [CommandSwitch("--account-scope")]
    public string? AccountScope { get; set; }

    [CommandSwitch("--lookback-period-in-days")]
    public string? LookbackPeriodInDays { get; set; }

    [CommandSwitch("--term-in-years")]
    public string? TermInYears { get; set; }

    [CommandSwitch("--payment-option")]
    public string? PaymentOption { get; set; }

    [CommandSwitch("--service-specification")]
    public string? ServiceSpecification { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--next-page-token")]
    public string? NextPageToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}