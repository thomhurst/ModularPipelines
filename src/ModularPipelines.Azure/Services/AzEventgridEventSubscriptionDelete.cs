using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "event-subscription")]
public class AzEventgridEventSubscriptionDelete
{
    public AzEventgridEventSubscriptionDelete(
        AzEventgridEventSubscriptionDeleteEventgrid eventgrid
    )
    {
        Eventgrid = eventgrid;
    }

    public AzEventgridEventSubscriptionDeleteEventgrid Eventgrid { get; }
}