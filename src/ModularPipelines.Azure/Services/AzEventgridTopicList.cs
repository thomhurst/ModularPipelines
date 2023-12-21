using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "topic")]
public class AzEventgridTopicList
{
    public AzEventgridTopicList(
        AzEventgridTopicListEventgrid eventgrid
    )
    {
        Eventgrid = eventgrid;
    }

    public AzEventgridTopicListEventgrid Eventgrid { get; }
}