using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "partner", "namespace")]
public class AzEventgridPartnerNamespaceList
{
    public AzEventgridPartnerNamespaceList(
        AzEventgridPartnerNamespaceListEventgrid eventgrid
    )
    {
        Eventgrid = eventgrid;
    }

    public AzEventgridPartnerNamespaceListEventgrid Eventgrid { get; }
}