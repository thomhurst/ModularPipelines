using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("servicebus", "topic", "authorization-rule", "list")]
public record AzServicebusTopicAuthorizationRuleListOptions(
[property: CliOption("--namespace-name")] string NamespaceName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--topic-name")] string TopicName
) : AzOptions;