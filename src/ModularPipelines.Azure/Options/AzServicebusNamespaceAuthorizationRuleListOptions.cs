using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("servicebus", "namespace", "authorization-rule", "list")]
public record AzServicebusNamespaceAuthorizationRuleListOptions(
[property: CliOption("--namespace-name")] string NamespaceName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;