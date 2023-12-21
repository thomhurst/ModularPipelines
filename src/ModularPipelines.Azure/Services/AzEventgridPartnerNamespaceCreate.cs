using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "partner", "namespace")]
public class AzEventgridPartnerNamespaceCreate
{
    public AzEventgridPartnerNamespaceCreate(
        AzEventgridPartnerNamespaceCreateEventgrid eventgrid
    )
    {
        Eventgrid = eventgrid;
    }

    public AzEventgridPartnerNamespaceCreateEventgrid Eventgrid { get; }
}