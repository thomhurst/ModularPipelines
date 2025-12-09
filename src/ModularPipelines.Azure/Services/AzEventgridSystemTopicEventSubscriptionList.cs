using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "system-topic", "event-subscription")]
public class AzEventgridSystemTopicEventSubscriptionList
{
    public AzEventgridSystemTopicEventSubscriptionList(
        AzEventgridSystemTopicEventSubscriptionListEventgrid eventgrid
    )
    {
        Eventgrid = eventgrid;
    }

    public AzEventgridSystemTopicEventSubscriptionListEventgrid Eventgrid { get; }
}