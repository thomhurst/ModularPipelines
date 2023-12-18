using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "application-gateway", "ssl-policy", "predefined", "list")]
public record AzNetworkApplicationGatewaySslPolicyPredefinedListOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
}