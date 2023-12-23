using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ce", "get-savings-plans-purchase-recommendation")]
public record AwsCeGetSavingsPlansPurchaseRecommendationOptions(
[property: CommandSwitch("--savings-plans-type")] string SavingsPlansType,
[property: CommandSwitch("--term-in-years")] string TermInYears,
[property: CommandSwitch("--payment-option")] string PaymentOption,
[property: CommandSwitch("--lookback-period-in-days")] string LookbackPeriodInDays
) : AwsOptions
{
    [CommandSwitch("--account-scope")]
    public string? AccountScope { get; set; }

    [CommandSwitch("--next-page-token")]
    public string? NextPageToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--filter")]
    public string? Filter { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}