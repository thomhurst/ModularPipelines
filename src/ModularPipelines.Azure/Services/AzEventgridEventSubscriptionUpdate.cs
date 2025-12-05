using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "event-subscription")]
public class AzEventgridEventSubscriptionUpdate
{
    public AzEventgridEventSubscriptionUpdate(
        AzEventgridEventSubscriptionUpdateEventgrid eventgrid
    )
    {
        Eventgrid = eventgrid;
    }

    public AzEventgridEventSubscriptionUpdateEventgrid Eventgrid { get; }
}