using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("blockchain", "member", "update")]
public record AzBlockchainMemberUpdateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--consortium-management-account-password")]
    public int? ConsortiumManagementAccountPassword { get; set; }

    [CliOption("--firewall-rules")]
    public string? FirewallRules { get; set; }

    [CliOption("--password")]
    public string? Password { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}