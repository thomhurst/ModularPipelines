using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("servicebus", "topic", "subscription", "list")]
public record AzServicebusTopicSubscriptionListOptions(
[property: CommandSwitch("--namespace-name")] string NamespaceName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--topic-name")] string TopicName
) : AzOptions
{
    [CommandSwitch("--skip")]
    public string? Skip { get; set; }

    [CommandSwitch("--top")]
    public string? Top { get; set; }
}