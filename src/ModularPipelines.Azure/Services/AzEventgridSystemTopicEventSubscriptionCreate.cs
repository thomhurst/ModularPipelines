using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "system-topic", "event-subscription")]
public class AzEventgridSystemTopicEventSubscriptionCreate
{
    public AzEventgridSystemTopicEventSubscriptionCreate(
        AzEventgridSystemTopicEventSubscriptionCreateEventgrid eventgrid
    )
    {
        Eventgrid = eventgrid;
    }

    public AzEventgridSystemTopicEventSubscriptionCreateEventgrid Eventgrid { get; }
}