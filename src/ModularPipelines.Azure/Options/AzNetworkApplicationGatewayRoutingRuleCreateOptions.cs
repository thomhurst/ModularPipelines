using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "application-gateway", "routing-rule", "create")]
public record AzNetworkApplicationGatewayRoutingRuleCreateOptions(
[property: CommandSwitch("--gateway-name")] string GatewayName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--address-pool")]
    public string? AddressPool { get; set; }

    [CommandSwitch("--listener")]
    public string? Listener { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--priority")]
    public string? Priority { get; set; }

    [CommandSwitch("--rule-type")]
    public string? RuleType { get; set; }

    [CommandSwitch("--settings")]
    public string? Settings { get; set; }
}

