using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "application-gateway", "ssl-policy", "predefined", "show")]
public record AzNetworkApplicationGatewaySslPolicyPredefinedShowOptions(
[property: CliOption("--name")] string Name
) : AzOptions;