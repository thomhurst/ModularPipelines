using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "topic-type")]
public class AzEventgridTopicTypeList
{
    public AzEventgridTopicTypeList(
        AzEventgridTopicTypeListEventgrid eventgrid
    )
    {
        Eventgrid = eventgrid;
    }

    public AzEventgridTopicTypeListEventgrid Eventgrid { get; }
}