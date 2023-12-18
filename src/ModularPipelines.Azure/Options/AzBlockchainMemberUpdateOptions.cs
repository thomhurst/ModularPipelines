using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("blockchain", "member", "update")]
public record AzBlockchainMemberUpdateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--consortium-management-account-password")]
    public int? ConsortiumManagementAccountPassword { get; set; }

    [CommandSwitch("--firewall-rules")]
    public string? FirewallRules { get; set; }

    [CommandSwitch("--password")]
    public string? Password { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}