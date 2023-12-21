using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "domain", "topic")]
public class AzEventgridDomainTopicList
{
    public AzEventgridDomainTopicList(
        AzEventgridDomainTopicListEventgrid eventgrid
    )
    {
        Eventgrid = eventgrid;
    }

    public AzEventgridDomainTopicListEventgrid Eventgrid { get; }
}