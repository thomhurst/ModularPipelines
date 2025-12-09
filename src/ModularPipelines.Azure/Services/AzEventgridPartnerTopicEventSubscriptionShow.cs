using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "partner", "topic", "event-subscription")]
public class AzEventgridPartnerTopicEventSubscriptionShow
{
    public AzEventgridPartnerTopicEventSubscriptionShow(
        AzEventgridPartnerTopicEventSubscriptionShowEventgrid eventgrid
    )
    {
        Eventgrid = eventgrid;
    }

    public AzEventgridPartnerTopicEventSubscriptionShowEventgrid Eventgrid { get; }
}