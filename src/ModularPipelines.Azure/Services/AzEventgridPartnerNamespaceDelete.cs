using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "partner", "namespace")]
public class AzEventgridPartnerNamespaceDelete
{
    public AzEventgridPartnerNamespaceDelete(
        AzEventgridPartnerNamespaceDeleteEventgrid eventgrid
    )
    {
        Eventgrid = eventgrid;
    }

    public AzEventgridPartnerNamespaceDeleteEventgrid Eventgrid { get; }
}