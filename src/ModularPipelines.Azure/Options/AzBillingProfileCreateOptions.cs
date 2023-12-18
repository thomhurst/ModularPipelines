using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("billing", "profile", "create")]
public record AzBillingProfileCreateOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--bill-to")]
    public string? BillTo { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [BooleanCommandSwitch("--enabled-azure-plans")]
    public bool? EnabledAzurePlans { get; set; }

    [BooleanCommandSwitch("--invoice-email-opt-in")]
    public bool? InvoiceEmailOptIn { get; set; }

    [CommandSwitch("--invoice-sections-value")]
    public string? InvoiceSectionsValue { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--po-number")]
    public string? PoNumber { get; set; }
}