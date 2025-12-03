using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "event-subscription")]
public class AzEventgridEventSubscriptionList
{
    public AzEventgridEventSubscriptionList(
        AzEventgridEventSubscriptionListEventgrid eventgrid
    )
    {
        Eventgrid = eventgrid;
    }

    public AzEventgridEventSubscriptionListEventgrid Eventgrid { get; }
}