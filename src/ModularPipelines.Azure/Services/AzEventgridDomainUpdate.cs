using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "domain")]
public class AzEventgridDomainUpdate
{
    public AzEventgridDomainUpdate(
        AzEventgridDomainUpdateEventgrid eventgrid
    )
    {
        Eventgrid = eventgrid;
    }

    public AzEventgridDomainUpdateEventgrid Eventgrid { get; }
}