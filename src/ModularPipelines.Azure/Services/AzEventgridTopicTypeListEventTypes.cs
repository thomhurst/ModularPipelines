using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "topic-type")]
public class AzEventgridTopicTypeListEventTypes
{
    public AzEventgridTopicTypeListEventTypes(
        AzEventgridTopicTypeListEventTypesEventgrid eventgrid
    )
    {
        Eventgrid = eventgrid;
    }

    public AzEventgridTopicTypeListEventTypesEventgrid Eventgrid { get; }
}