using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "application-gateway", "rule", "create")]
public record AzNetworkApplicationGatewayRuleCreateOptions(
[property: CliOption("--gateway-name")] string GatewayName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--address-pool")]
    public string? AddressPool { get; set; }

    [CliOption("--http-listener")]
    public string? HttpListener { get; set; }

    [CliOption("--http-settings")]
    public string? HttpSettings { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--priority")]
    public string? Priority { get; set; }

    [CliOption("--redirect-config")]
    public string? RedirectConfig { get; set; }

    [CliOption("--rewrite-rule-set")]
    public string? RewriteRuleSet { get; set; }

    [CliOption("--rule-type")]
    public string? RuleType { get; set; }

    [CliOption("--url-path-map")]
    public string? UrlPathMap { get; set; }
}