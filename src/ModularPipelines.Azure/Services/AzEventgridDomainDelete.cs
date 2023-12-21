using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "domain")]
public class AzEventgridDomainDelete
{
    public AzEventgridDomainDelete(
        AzEventgridDomainDeleteEventgrid eventgrid
    )
    {
        Eventgrid = eventgrid;
    }

    public AzEventgridDomainDeleteEventgrid Eventgrid { get; }
}