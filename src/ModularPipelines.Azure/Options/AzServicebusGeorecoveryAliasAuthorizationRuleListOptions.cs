using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("servicebus", "georecovery-alias", "authorization-rule", "list")]
public record AzServicebusGeorecoveryAliasAuthorizationRuleListOptions(
[property: CommandSwitch("--alias")] string Alias,
[property: CommandSwitch("--namespace-name")] string NamespaceName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;