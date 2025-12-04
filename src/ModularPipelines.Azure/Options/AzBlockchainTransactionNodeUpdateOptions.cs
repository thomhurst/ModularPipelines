using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("blockchain", "transaction-node", "update")]
public record AzBlockchainTransactionNodeUpdateOptions(
[property: CliOption("--member-name")] string MemberName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--firewall-rules")]
    public string? FirewallRules { get; set; }

    [CliOption("--password")]
    public string? Password { get; set; }
}