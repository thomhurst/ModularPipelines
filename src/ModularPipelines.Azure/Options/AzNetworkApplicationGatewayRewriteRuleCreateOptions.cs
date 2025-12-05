using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "application-gateway", "rewrite-rule", "create")]
public record AzNetworkApplicationGatewayRewriteRuleCreateOptions(
[property: CliOption("--gateway-name")] string GatewayName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--rule-set-name")] string RuleSetName
) : AzOptions
{
    [CliOption("--conditions")]
    public string? Conditions { get; set; }

    [CliFlag("--enable-reroute")]
    public bool? EnableReroute { get; set; }

    [CliOption("--modified-path")]
    public string? ModifiedPath { get; set; }

    [CliOption("--modified-query-string")]
    public string? ModifiedQueryString { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--request-headers")]
    public string? RequestHeaders { get; set; }

    [CliOption("--response-headers")]
    public string? ResponseHeaders { get; set; }

    [CliOption("--sequence")]
    public string? Sequence { get; set; }
}