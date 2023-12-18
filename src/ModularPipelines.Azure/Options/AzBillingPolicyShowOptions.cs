using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("billing", "policy", "show")]
public record AzBillingPolicyShowOptions(
[property: CommandSwitch("--account-name")] int AccountName
) : AzOptions
{
    [CommandSwitch("--customer-name")]
    public string? CustomerName { get; set; }

    [CommandSwitch("--profile-name")]
    public string? ProfileName { get; set; }
}

