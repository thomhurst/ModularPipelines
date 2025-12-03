using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("relay", "hyco", "authorization-rule", "keys", "list")]
public record AzRelayHycoAuthorizationRuleKeysListOptions(
[property: CliOption("--hybrid-connection-name")] string HybridConnectionName,
[property: CliOption("--name")] string Name,
[property: CliOption("--namespace-name")] string NamespaceName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;