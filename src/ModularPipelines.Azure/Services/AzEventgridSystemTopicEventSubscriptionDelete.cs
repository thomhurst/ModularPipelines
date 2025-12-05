using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "system-topic", "event-subscription")]
public class AzEventgridSystemTopicEventSubscriptionDelete
{
    public AzEventgridSystemTopicEventSubscriptionDelete(
        AzEventgridSystemTopicEventSubscriptionDeleteEventgrid eventgrid
    )
    {
        Eventgrid = eventgrid;
    }

    public AzEventgridSystemTopicEventSubscriptionDeleteEventgrid Eventgrid { get; }
}