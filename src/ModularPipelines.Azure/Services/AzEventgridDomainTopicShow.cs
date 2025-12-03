using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "domain", "topic")]
public class AzEventgridDomainTopicShow
{
    public AzEventgridDomainTopicShow(
        AzEventgridDomainTopicShowEventgrid eventgrid
    )
    {
        Eventgrid = eventgrid;
    }

    public AzEventgridDomainTopicShowEventgrid Eventgrid { get; }
}