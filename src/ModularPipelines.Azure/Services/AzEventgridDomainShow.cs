using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "domain")]
public class AzEventgridDomainShow
{
    public AzEventgridDomainShow(
        AzEventgridDomainShowEventgrid eventgrid
    )
    {
        Eventgrid = eventgrid;
    }

    public AzEventgridDomainShowEventgrid Eventgrid { get; }
}