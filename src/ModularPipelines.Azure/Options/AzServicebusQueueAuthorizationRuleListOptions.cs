using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("servicebus", "queue", "authorization-rule", "list")]
public record AzServicebusQueueAuthorizationRuleListOptions(
[property: CliOption("--namespace-name")] string NamespaceName,
[property: CliOption("--queue-name")] string QueueName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;