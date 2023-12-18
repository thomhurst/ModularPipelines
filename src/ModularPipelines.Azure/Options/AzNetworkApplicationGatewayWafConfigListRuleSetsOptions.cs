using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "application-gateway", "waf-config", "list-rule-sets")]
public record AzNetworkApplicationGatewayWafConfigListRuleSetsOptions(
[property: BooleanCommandSwitch("--enabled")] bool Enabled
) : AzOptions
{
    [CommandSwitch("--group")]
    public string? Group { get; set; }

    [CommandSwitch("--type")]
    public string? Type { get; set; }

    [CommandSwitch("--version")]
    public string? Version { get; set; }
}

