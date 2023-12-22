using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "front-door", "waf-policy", "list")]
public record AzNetworkFrontDoorWafPolicyListOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;