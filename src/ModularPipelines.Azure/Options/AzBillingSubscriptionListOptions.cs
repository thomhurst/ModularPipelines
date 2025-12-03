using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("billing", "subscription", "list")]
public record AzBillingSubscriptionListOptions(
[property: CliOption("--account-name")] int AccountName
) : AzOptions
{
    [CliOption("--customer-name")]
    public string? CustomerName { get; set; }

    [CliOption("--invoice-section-name")]
    public string? InvoiceSectionName { get; set; }

    [CliOption("--profile-name")]
    public string? ProfileName { get; set; }
}