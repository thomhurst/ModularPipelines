using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "partner", "namespace", "event-channel")]
public class AzEventgridPartnerNamespaceEventChannelDelete
{
    public AzEventgridPartnerNamespaceEventChannelDelete(
        AzEventgridPartnerNamespaceEventChannelDeleteEventgrid eventgrid
    )
    {
        Eventgrid = eventgrid;
    }

    public AzEventgridPartnerNamespaceEventChannelDeleteEventgrid Eventgrid { get; }
}