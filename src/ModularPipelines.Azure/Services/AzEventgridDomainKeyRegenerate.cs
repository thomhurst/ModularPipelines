using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "domain", "key")]
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