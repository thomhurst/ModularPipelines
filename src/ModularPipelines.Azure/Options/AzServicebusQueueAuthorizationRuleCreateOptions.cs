using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("servicebus", "queue", "authorization-rule", "create")]
public record AzServicebusQueueAuthorizationRuleCreateOptions(
[property: CommandSwitch("--authorization-rule-name")] string AuthorizationRuleName,
[property: CommandSwitch("--namespace-name")] string NamespaceName,
[property: CommandSwitch("--queue-name")] string QueueName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--rights")]
    public string? Rights { get; set; }
}

