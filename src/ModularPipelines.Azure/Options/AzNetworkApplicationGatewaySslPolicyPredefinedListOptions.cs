using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "application-gateway", "ssl-policy", "predefined", "list")]
public record AzNetworkApplicationGatewaySslPolicyPredefinedListOptions : AzOptions;