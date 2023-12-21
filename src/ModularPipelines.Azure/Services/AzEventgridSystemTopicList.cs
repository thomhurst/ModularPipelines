using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "system-topic")]
public class AzEventgridSystemTopicList
{
    public AzEventgridSystemTopicList(
        AzEventgridSystemTopicListEventgrid eventgrid
    )
    {
        Eventgrid = eventgrid;
    }

    public AzEventgridSystemTopicListEventgrid Eventgrid { get; }
}