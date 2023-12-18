using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("billing", "product", "update")]
public record AzBillingProductUpdateOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--auto-renew")]
    public string? AutoRenew { get; set; }

    [CommandSwitch("--billing-frequency")]
    public string? BillingFrequency { get; set; }

    [CommandSwitch("--status")]
    public string? Status { get; set; }
}