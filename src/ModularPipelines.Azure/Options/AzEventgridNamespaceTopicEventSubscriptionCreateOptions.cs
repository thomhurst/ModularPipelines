using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "namespace", "topic", "event-subscription", "create")]
public record AzEventgridNamespaceTopicEventSubscriptionCreateOptions(
[property: CliOption("--event-subscription-name")] string EventSubscriptionName,
[property: CliOption("--namespace-name")] string NamespaceName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--topic-name")] string TopicName
) : AzOptions
{
    [CliOption("--delivery-configuration")]
    public string? DeliveryConfiguration { get; set; }

    [CliOption("--event-delivery-schema")]
    public string? EventDeliverySchema { get; set; }

    [CliOption("--filters-configuration")]
    public string? FiltersConfiguration { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}