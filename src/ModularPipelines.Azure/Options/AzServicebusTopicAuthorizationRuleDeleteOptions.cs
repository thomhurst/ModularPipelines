using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("servicebus", "topic", "authorization-rule", "delete")]
public record AzServicebusTopicAuthorizationRuleDeleteOptions : AzOptions
{
    [CommandSwitch("--authorization-rule-name")]
    public string? AuthorizationRuleName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--namespace-name")]
    public string? NamespaceName { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--topic-name")]
    public string? TopicName { get; set; }
}