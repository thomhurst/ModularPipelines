using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dls", "account", "network-rule", "list")]
public record AzDlsAccountNetworkRuleListOptions(
[property: CommandSwitch("--account-name")] int AccountName
) : AzOptions
{
    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}