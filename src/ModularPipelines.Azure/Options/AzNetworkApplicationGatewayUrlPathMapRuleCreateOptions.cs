using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "application-gateway", "url-path-map", "rule", "create")]
public record AzNetworkApplicationGatewayUrlPathMapRuleCreateOptions(
[property: CliOption("--gateway-name")] string GatewayName,
[property: CliOption("--name")] string Name,
[property: CliOption("--path-map-name")] string PathMapName,
[property: CliOption("--paths")] string Paths,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--address-pool")]
    public string? AddressPool { get; set; }

    [CliOption("--http-settings")]
    public string? HttpSettings { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--redirect-config")]
    public string? RedirectConfig { get; set; }

    [CliOption("--rewrite-rule-set")]
    public string? RewriteRuleSet { get; set; }

    [CliOption("--waf-policy")]
    public string? WafPolicy { get; set; }
}