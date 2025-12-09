using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "event-subscription")]
public class AzEventgridEventSubscriptionCreate
{
    public AzEventgridEventSubscriptionCreate(
        AzEventgridEventSubscriptionCreateEventgrid eventgrid
    )
    {
        Eventgrid = eventgrid;
    }

    public AzEventgridEventSubscriptionCreateEventgrid Eventgrid { get; }
}