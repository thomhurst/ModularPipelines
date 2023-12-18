using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("servicebus", "topic", "subscription", "rule", "list")]
public record AzServicebusTopicSubscriptionRuleListOptions(
[property: CommandSwitch("--namespace-name")] string NamespaceName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--subscription-name")] string SubscriptionName,
[property: CommandSwitch("--topic-name")] string TopicName
) : AzOptions
{
    [CommandSwitch("--skip")]
    public string? Skip { get; set; }

    [CommandSwitch("--top")]
    public string? Top { get; set; }
}