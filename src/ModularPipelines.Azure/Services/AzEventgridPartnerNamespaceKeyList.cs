using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "partner", "namespace", "key")]
public class AzEventgridPartnerNamespaceKeyList
{
    public AzEventgridPartnerNamespaceKeyList(
        AzEventgridPartnerNamespaceKeyListEventgrid eventgrid
    )
    {
        Eventgrid = eventgrid;
    }

    public AzEventgridPartnerNamespaceKeyListEventgrid Eventgrid { get; }
}