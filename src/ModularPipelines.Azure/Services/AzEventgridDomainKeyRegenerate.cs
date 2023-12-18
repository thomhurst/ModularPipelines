using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "domain", "key")]
public class AzEventgridDomainKeyRegenerate
{
    public AzEventgridDomainKeyRegenerate(
        AzEventgridDomainKeyRegenerateEventgrid eventgrid
    )
    {
        Eventgrid = eventgrid;
    }

    public AzEventgridDomainKeyRegenerateEventgrid Eventgrid { get; }
}