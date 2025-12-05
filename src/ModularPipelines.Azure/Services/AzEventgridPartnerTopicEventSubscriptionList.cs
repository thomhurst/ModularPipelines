using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "partner", "topic", "event-subscription")]
public class AzEventgridPartnerTopicEventSubscriptionList
{
    public AzEventgridPartnerTopicEventSubscriptionList(
        AzEventgridPartnerTopicEventSubscriptionListEventgrid eventgrid
    )
    {
        Eventgrid = eventgrid;
    }

    public AzEventgridPartnerTopicEventSubscriptionListEventgrid Eventgrid { get; }
}