using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "partner", "namespace")]
public class AzEventgridPartnerNamespaceEventChannel
{
    public AzEventgridPartnerNamespaceEventChannel(
        AzEventgridPartnerNamespaceEventChannelCreate create,
        AzEventgridPartnerNamespaceEventChannelDelete delete,
        AzEventgridPartnerNamespaceEventChannelList list,
        AzEventgridPartnerNamespaceEventChannelShow show
    )
    {
        Create = create;
        Delete = delete;
        List = list;
        Show = show;
    }

    public AzEventgridPartnerNamespaceEventChannelCreate Create { get; }

    public AzEventgridPartnerNamespaceEventChannelDelete Delete { get; }

    public AzEventgridPartnerNamespaceEventChannelList List { get; }

    public AzEventgridPartnerNamespaceEventChannelShow Show { get; }
}