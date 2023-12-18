using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("blockchain", "member", "create")]
public record AzBlockchainMemberCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--consortium")]
    public string? Consortium { get; set; }

    [CommandSwitch("--consortium-management-account-password")]
    public int? ConsortiumManagementAccountPassword { get; set; }

    [CommandSwitch("--firewall-rules")]
    public string? FirewallRules { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--password")]
    public string? Password { get; set; }

    [CommandSwitch("--protocol")]
    public string? Protocol { get; set; }

    [CommandSwitch("--sku")]
    public string? Sku { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--validator-nodes-sku")]
    public string? ValidatorNodesSku { get; set; }
}