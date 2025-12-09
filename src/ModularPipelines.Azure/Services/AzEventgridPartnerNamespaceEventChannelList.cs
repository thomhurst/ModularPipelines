using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "partner", "namespace", "event-channel")]
public class AzEventgridPartnerNamespaceEventChannelList
{
    public AzEventgridPartnerNamespaceEventChannelList(
        AzEventgridPartnerNamespaceEventChannelListEventgrid eventgrid
    )
    {
        Eventgrid = eventgrid;
    }

    public AzEventgridPartnerNamespaceEventChannelListEventgrid Eventgrid { get; }
}