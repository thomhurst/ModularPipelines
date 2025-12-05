using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "partner", "namespace")]
public class AzEventgridPartnerNamespaceShow
{
    public AzEventgridPartnerNamespaceShow(
        AzEventgridPartnerNamespaceShowEventgrid eventgrid
    )
    {
        Eventgrid = eventgrid;
    }

    public AzEventgridPartnerNamespaceShowEventgrid Eventgrid { get; }
}