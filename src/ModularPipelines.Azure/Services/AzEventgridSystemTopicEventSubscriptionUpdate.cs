using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "system-topic", "event-subscription")]
public class AzEventgridSystemTopicEventSubscriptionUpdate
{
    public AzEventgridSystemTopicEventSubscriptionUpdate(
        AzEventgridSystemTopicEventSubscriptionUpdateEventgrid eventgrid
    )
    {
        Eventgrid = eventgrid;
    }

    public AzEventgridSystemTopicEventSubscriptionUpdateEventgrid Eventgrid { get; }
}