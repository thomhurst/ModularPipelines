using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("blockchain", "member", "create")]
public record AzBlockchainMemberCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--consortium")]
    public string? Consortium { get; set; }

    [CliOption("--consortium-management-account-password")]
    public int? ConsortiumManagementAccountPassword { get; set; }

    [CliOption("--firewall-rules")]
    public string? FirewallRules { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--password")]
    public string? Password { get; set; }

    [CliOption("--protocol")]
    public string? Protocol { get; set; }

    [CliOption("--sku")]
    public string? Sku { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--validator-nodes-sku")]
    public string? ValidatorNodesSku { get; set; }
}