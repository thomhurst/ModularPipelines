using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "domain", "topic")]
public class AzEventgridDomainTopicDelete
{
    public AzEventgridDomainTopicDelete(
        AzEventgridDomainTopicDeleteEventgrid eventgrid
    )
    {
        Eventgrid = eventgrid;
    }

    public AzEventgridDomainTopicDeleteEventgrid Eventgrid { get; }
}