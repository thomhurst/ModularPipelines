using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "domain")]
public class AzEventgridDomainList
{
    public AzEventgridDomainList(
        AzEventgridDomainListEventgrid eventgrid
    )
    {
        Eventgrid = eventgrid;
    }

    public AzEventgridDomainListEventgrid Eventgrid { get; }
}