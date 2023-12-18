using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("billing", "customer", "list")]
public record AzBillingCustomerListOptions(
[property: CommandSwitch("--account-name")] int AccountName
) : AzOptions
{
    [CommandSwitch("--filter")]
    public string? Filter { get; set; }

    [CommandSwitch("--profile-name")]
    public string? ProfileName { get; set; }

    [CommandSwitch("--search")]
    public string? Search { get; set; }
}