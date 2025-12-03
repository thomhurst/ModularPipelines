using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "application-gateway", "waf-config", "list-rule-sets")]
public record AzNetworkApplicationGatewayWafConfigListRuleSetsOptions : AzOptions
{
    [CliOption("--group")]
    public string? Group { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }

    [CliOption("--version")]
    public string? Version { get; set; }
}