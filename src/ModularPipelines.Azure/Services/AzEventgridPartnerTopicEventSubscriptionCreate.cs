using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "partner", "topic", "event-subscription")]
public class AzEventgridPartnerTopicEventSubscriptionCreate
{
    public AzEventgridPartnerTopicEventSubscriptionCreate(
        AzEventgridPartnerTopicEventSubscriptionCreateEventgrid eventgrid
    )
    {
        Eventgrid = eventgrid;
    }

    public AzEventgridPartnerTopicEventSubscriptionCreateEventgrid Eventgrid { get; }
}