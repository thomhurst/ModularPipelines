using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "domain", "topic")]
public class AzEventgridDomainTopicCreate
{
    public AzEventgridDomainTopicCreate(
        AzEventgridDomainTopicCreateEventgrid eventgrid
    )
    {
        Eventgrid = eventgrid;
    }

    public AzEventgridDomainTopicCreateEventgrid Eventgrid { get; }
}