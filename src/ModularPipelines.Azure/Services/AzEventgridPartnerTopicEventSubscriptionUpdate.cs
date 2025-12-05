using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "partner", "topic", "event-subscription")]
public class AzEventgridPartnerTopicEventSubscriptionUpdate
{
    public AzEventgridPartnerTopicEventSubscriptionUpdate(
        AzEventgridPartnerTopicEventSubscriptionUpdateEventgrid eventgrid
    )
    {
        Eventgrid = eventgrid;
    }

    public AzEventgridPartnerTopicEventSubscriptionUpdateEventgrid Eventgrid { get; }
}