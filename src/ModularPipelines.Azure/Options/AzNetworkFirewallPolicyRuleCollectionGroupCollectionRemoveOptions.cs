using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "firewall", "policy", "rule-collection-group", "collection", "remove")]
public record AzNetworkFirewallPolicyRuleCollectionGroupCollectionRemoveOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--policy-name")] string PolicyName,
[property: CliOption("--rcg-name")] string RcgName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }
}