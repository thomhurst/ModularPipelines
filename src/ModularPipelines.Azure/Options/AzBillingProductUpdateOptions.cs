using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("billing", "product", "update")]
public record AzBillingProductUpdateOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--auto-renew")]
    public string? AutoRenew { get; set; }

    [CliOption("--billing-frequency")]
    public string? BillingFrequency { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }
}