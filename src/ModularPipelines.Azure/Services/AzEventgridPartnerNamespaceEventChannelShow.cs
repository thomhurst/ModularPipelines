using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "partner", "namespace", "event-channel")]
public class AzEventgridPartnerNamespaceEventChannelShow
{
    public AzEventgridPartnerNamespaceEventChannelShow(
        AzEventgridPartnerNamespaceEventChannelShowEventgrid eventgrid
    )
    {
        Eventgrid = eventgrid;
    }

    public AzEventgridPartnerNamespaceEventChannelShowEventgrid Eventgrid { get; }
}