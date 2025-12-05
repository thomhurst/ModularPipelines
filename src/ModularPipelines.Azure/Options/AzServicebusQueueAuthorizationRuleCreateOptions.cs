using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("servicebus", "queue", "authorization-rule", "create")]
public record AzServicebusQueueAuthorizationRuleCreateOptions(
[property: CliOption("--authorization-rule-name")] string AuthorizationRuleName,
[property: CliOption("--namespace-name")] string NamespaceName,
[property: CliOption("--queue-name")] string QueueName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--rights")]
    public string? Rights { get; set; }
}