using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "application-gateway", "waf-config", "list-rule-sets")]
public record AzNetworkApplicationGatewayWafConfigListRuleSetsOptions : AzOptions
{
    [CommandSwitch("--group")]
    public string? Group { get; set; }

    [CommandSwitch("--type")]
    public string? Type { get; set; }

    [CommandSwitch("--version")]
    public string? Version { get; set; }
}