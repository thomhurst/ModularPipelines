using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("relay", "wcfrelay", "authorization-rule", "keys", "list")]
public record AzRelayWcfrelayAuthorizationRuleKeysListOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--namespace-name")] string NamespaceName,
[property: CommandSwitch("--relay-name")] string RelayName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
}