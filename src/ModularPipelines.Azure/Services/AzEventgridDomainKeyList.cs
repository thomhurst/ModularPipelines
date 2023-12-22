using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

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