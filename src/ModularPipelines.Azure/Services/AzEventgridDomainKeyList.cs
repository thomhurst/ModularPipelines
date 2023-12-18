using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "domain", "key")]
public class AzEventgridDomainKeyList
{
    public AzEventgridDomainKeyList(
        AzEventgridDomainKeyListEventgrid eventgrid
    )
    {
        Eventgrid = eventgrid;
    }

    public AzEventgridDomainKeyListEventgrid Eventgrid { get; }
}

