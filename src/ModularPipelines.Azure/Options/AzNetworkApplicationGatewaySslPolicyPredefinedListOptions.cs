using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "application-gateway", "ssl-policy", "predefined", "list")]
public record AzNetworkApplicationGatewaySslPolicyPredefinedListOptions : AzOptions;