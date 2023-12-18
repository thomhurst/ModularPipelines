using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "namespace", "topic", "event-subscription", "create")]
public record AzEventgridNamespaceTopicEventSubscriptionCreateOptions(
[property: CommandSwitch("--event-subscription-name")] string EventSubscriptionName,
[property: CommandSwitch("--namespace-name")] string NamespaceName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--topic-name")] string TopicName
) : AzOptions
{
    [CommandSwitch("--delivery-configuration")]
    public string? DeliveryConfiguration { get; set; }

    [CommandSwitch("--event-delivery-schema")]
    public string? EventDeliverySchema { get; set; }

    [CommandSwitch("--filters-configuration")]
    public string? FiltersConfiguration { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }
}