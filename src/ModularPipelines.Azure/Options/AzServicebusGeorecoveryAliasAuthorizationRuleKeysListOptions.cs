using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("servicebus", "georecovery-alias", "authorization-rule", "keys", "list")]
public record AzServicebusGeorecoveryAliasAuthorizationRuleKeysListOptions(
[property: CliOption("--alias")] string Alias,
[property: CliOption("--authorization-rule-name")] string AuthorizationRuleName,
[property: CliOption("--namespace-name")] string NamespaceName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;