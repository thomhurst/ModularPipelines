using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "domain")]
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