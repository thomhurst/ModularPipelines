using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("billing", "profile", "create")]
public record AzBillingProfileCreateOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--bill-to")]
    public string? BillTo { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliFlag("--enabled-azure-plans")]
    public bool? EnabledAzurePlans { get; set; }

    [CliFlag("--invoice-email-opt-in")]
    public bool? InvoiceEmailOptIn { get; set; }

    [CliOption("--invoice-sections-value")]
    public string? InvoiceSectionsValue { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--po-number")]
    public string? PoNumber { get; set; }
}