using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "namespace", "topic", "event-subscription", "update")]
public record AzEventgridNamespaceTopicEventSubscriptionUpdateOptions : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--delivery-configuration")]
    public string? DeliveryConfiguration { get; set; }

    [CliOption("--event-delivery-schema")]
    public string? EventDeliverySchema { get; set; }

    [CliOption("--event-subscription-name")]
    public string? EventSubscriptionName { get; set; }

    [CliOption("--filters-configuration")]
    public string? FiltersConfiguration { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--namespace-name")]
    public string? NamespaceName { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--topic-name")]
    public string? TopicName { get; set; }
}