using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "application-gateway", "routing-rule", "create")]
public record AzNetworkApplicationGatewayRoutingRuleCreateOptions(
[property: CliOption("--gateway-name")] string GatewayName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--address-pool")]
    public string? AddressPool { get; set; }

    [CliOption("--listener")]
    public string? Listener { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--priority")]
    public string? Priority { get; set; }

    [CliOption("--rule-type")]
    public string? RuleType { get; set; }

    [CliOption("--settings")]
    public string? Settings { get; set; }
}