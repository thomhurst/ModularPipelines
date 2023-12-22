using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "application-gateway", "ssl-policy", "predefined", "show")]
public record AzNetworkApplicationGatewaySslPolicyPredefinedShowOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions;