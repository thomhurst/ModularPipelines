using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dla", "account", "firewall", "create")]
public record AzDlaAccountFirewallCreateOptions(
[property: CliOption("--account")] int Account,
[property: CliOption("--end-ip-address")] string EndIpAddress,
[property: CliOption("--firewall-rule-name")] string FirewallRuleName,
[property: CliOption("--start-ip-address")] string StartIpAddress
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}