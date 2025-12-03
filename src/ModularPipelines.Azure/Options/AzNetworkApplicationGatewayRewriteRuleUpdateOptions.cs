using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "application-gateway", "rewrite-rule", "update")]
public record AzNetworkApplicationGatewayRewriteRuleUpdateOptions(
[property: CliOption("--gateway-name")] string GatewayName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--rule-set-name")] string RuleSetName
) : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--conditions")]
    public string? Conditions { get; set; }

    [CliFlag("--enable-reroute")]
    public bool? EnableReroute { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--modified-path")]
    public string? ModifiedPath { get; set; }

    [CliOption("--modified-query-string")]
    public string? ModifiedQueryString { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--request-headers")]
    public string? RequestHeaders { get; set; }

    [CliOption("--response-headers")]
    public string? ResponseHeaders { get; set; }

    [CliOption("--sequence")]
    public string? Sequence { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }
}