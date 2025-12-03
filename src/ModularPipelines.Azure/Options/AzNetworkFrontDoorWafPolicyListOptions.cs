using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "front-door", "waf-policy", "list")]
public record AzNetworkFrontDoorWafPolicyListOptions(
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;