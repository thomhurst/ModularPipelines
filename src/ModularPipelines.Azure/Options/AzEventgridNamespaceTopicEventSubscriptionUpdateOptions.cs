using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "namespace", "topic", "event-subscription", "update")]
public record AzEventgridNamespaceTopicEventSubscriptionUpdateOptions : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [CommandSwitch("--delivery-configuration")]
    public string? DeliveryConfiguration { get; set; }

    [CommandSwitch("--event-delivery-schema")]
    public string? EventDeliverySchema { get; set; }

    [CommandSwitch("--event-subscription-name")]
    public string? EventSubscriptionName { get; set; }

    [CommandSwitch("--filters-configuration")]
    public string? FiltersConfiguration { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--namespace-name")]
    public string? NamespaceName { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }

    [CommandSwitch("--topic-name")]
    public string? TopicName { get; set; }
}