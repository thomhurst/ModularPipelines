using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "application-gateway", "url-path-map", "update")]
public record AzNetworkApplicationGatewayUrlPathMapUpdateOptions(
[property: CliOption("--gateway-name")] string GatewayName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--default-address-pool")]
    public string? DefaultAddressPool { get; set; }

    [CliOption("--default-http-settings")]
    public string? DefaultHttpSettings { get; set; }

    [CliOption("--default-redirect-config")]
    public string? DefaultRedirectConfig { get; set; }

    [CliOption("--default-rewrite-rule-set")]
    public string? DefaultRewriteRuleSet { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--rules")]
    public string? Rules { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }
}