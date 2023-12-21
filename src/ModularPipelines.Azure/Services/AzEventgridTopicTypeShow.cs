using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "topic-type")]
public class AzEventgridTopicTypeShow
{
    public AzEventgridTopicTypeShow(
        AzEventgridTopicTypeShowEventgrid eventgrid
    )
    {
        Eventgrid = eventgrid;
    }

    public AzEventgridTopicTypeShowEventgrid Eventgrid { get; }
}