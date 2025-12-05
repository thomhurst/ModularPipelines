using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("billing", "policy", "update")]
public record AzBillingPolicyUpdateOptions(
[property: CliOption("--account-name")] int AccountName
) : AzOptions
{
    [CliOption("--customer-name")]
    public string? CustomerName { get; set; }

    [CliOption("--marketplace-purchases")]
    public string? MarketplacePurchases { get; set; }

    [CliOption("--profile-name")]
    public string? ProfileName { get; set; }

    [CliOption("--reservation-purchases")]
    public string? ReservationPurchases { get; set; }

    [CliOption("--view-charges")]
    public string? ViewCharges { get; set; }
}