using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("blockchain", "transaction-node", "update")]
public record AzBlockchainTransactionNodeUpdateOptions(
[property: CommandSwitch("--member-name")] string MemberName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--firewall-rules")]
    public string? FirewallRules { get; set; }

    [CommandSwitch("--password")]
    public string? Password { get; set; }
}