using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "namespace", "topic", "event-subscription", "delete")]
public record AzEventgridNamespaceTopicEventSubscriptionDeleteOptions : AzOptions
{
    [CliOption("--event-subscription-name")]
    public string? EventSubscriptionName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--namespace-name")]
    public string? NamespaceName { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--topic-name")]
    public string? TopicName { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}