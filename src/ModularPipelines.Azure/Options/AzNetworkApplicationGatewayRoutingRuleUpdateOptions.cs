using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "application-gateway", "routing-rule", "update")]
public record AzNetworkApplicationGatewayRoutingRuleUpdateOptions(
[property: CliOption("--gateway-name")] string GatewayName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--address-pool")]
    public string? AddressPool { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--listener")]
    public string? Listener { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--priority")]
    public string? Priority { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--rule-type")]
    public string? RuleType { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--settings")]
    public string? Settings { get; set; }
}