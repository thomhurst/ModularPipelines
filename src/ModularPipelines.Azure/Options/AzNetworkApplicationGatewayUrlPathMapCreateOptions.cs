using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "application-gateway", "url-path-map", "create")]
public record AzNetworkApplicationGatewayUrlPathMapCreateOptions(
[property: CliOption("--gateway-name")] string GatewayName,
[property: CliOption("--name")] string Name,
[property: CliOption("--paths")] string Paths,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--address-pool")]
    public string? AddressPool { get; set; }

    [CliOption("--default-address-pool")]
    public string? DefaultAddressPool { get; set; }

    [CliOption("--default-http-settings")]
    public string? DefaultHttpSettings { get; set; }

    [CliOption("--default-redirect-config")]
    public string? DefaultRedirectConfig { get; set; }

    [CliOption("--default-rewrite-rule-set")]
    public string? DefaultRewriteRuleSet { get; set; }

    [CliOption("--http-settings")]
    public string? HttpSettings { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--redirect-config")]
    public string? RedirectConfig { get; set; }

    [CliOption("--rewrite-rule-set")]
    public string? RewriteRuleSet { get; set; }

    [CliOption("--rule-name")]
    public string? RuleName { get; set; }

    [CliOption("--rules")]
    public string? Rules { get; set; }

    [CliOption("--waf-policy")]
    public string? WafPolicy { get; set; }
}