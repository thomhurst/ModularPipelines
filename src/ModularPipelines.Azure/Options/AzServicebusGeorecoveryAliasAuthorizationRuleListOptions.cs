using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("servicebus", "georecovery-alias", "authorization-rule", "list")]
public record AzServicebusGeorecoveryAliasAuthorizationRuleListOptions(
[property: CliOption("--alias")] string Alias,
[property: CliOption("--namespace-name")] string NamespaceName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;