using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("servicebus", "topic", "authorization-rule", "create")]
public record AzServicebusTopicAuthorizationRuleCreateOptions(
[property: CliOption("--authorization-rule-name")] string AuthorizationRuleName,
[property: CliOption("--namespace-name")] string NamespaceName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--topic-name")] string TopicName
) : AzOptions
{
    [CliOption("--rights")]
    public string? Rights { get; set; }
}