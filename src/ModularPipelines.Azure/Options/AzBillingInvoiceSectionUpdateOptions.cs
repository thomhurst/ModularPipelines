using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("billing", "invoice", "section", "update")]
public record AzBillingInvoiceSectionUpdateOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--invoice-section-name")] string InvoiceSectionName,
[property: CommandSwitch("--profile-name")] string ProfileName
) : AzOptions
{
    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--labels")]
    public string? Labels { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }
}