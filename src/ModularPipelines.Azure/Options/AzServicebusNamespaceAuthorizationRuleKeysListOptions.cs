using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("servicebus", "namespace", "authorization-rule", "keys", "list")]
public record AzServicebusNamespaceAuthorizationRuleKeysListOptions(
[property: CliOption("--authorization-rule-name")] string AuthorizationRuleName,
[property: CliOption("--namespace-name")] string NamespaceName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;