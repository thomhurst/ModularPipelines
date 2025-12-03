using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("relay", "wcfrelay", "authorization-rule", "keys", "list")]
public record AzRelayWcfrelayAuthorizationRuleKeysListOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--namespace-name")] string NamespaceName,
[property: CliOption("--relay-name")] string RelayName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;