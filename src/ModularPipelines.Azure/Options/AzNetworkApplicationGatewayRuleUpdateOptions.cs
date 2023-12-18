using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "application-gateway", "rule", "update")]
public record AzNetworkApplicationGatewayRuleUpdateOptions(
[property: CommandSwitch("--gateway-name")] string GatewayName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [CommandSwitch("--address-pool")]
    public string? AddressPool { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--http-listener")]
    public string? HttpListener { get; set; }

    [CommandSwitch("--http-settings")]
    public string? HttpSettings { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--priority")]
    public string? Priority { get; set; }

    [CommandSwitch("--redirect-config")]
    public string? RedirectConfig { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--rewrite-rule-set")]
    public string? RewriteRuleSet { get; set; }

    [CommandSwitch("--rule-type")]
    public string? RuleType { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--url-path-map")]
    public string? UrlPathMap { get; set; }
}