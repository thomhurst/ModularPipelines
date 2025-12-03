using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("servicebus", "queue", "authorization-rule", "keys", "list")]
public record AzServicebusQueueAuthorizationRuleKeysListOptions(
[property: CliOption("--authorization-rule-name")] string AuthorizationRuleName,
[property: CliOption("--namespace-name")] string NamespaceName,
[property: CliOption("--queue-name")] string QueueName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;