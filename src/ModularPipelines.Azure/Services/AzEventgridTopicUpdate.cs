using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "topic")]
public class AzEventgridTopicUpdate
{
    public AzEventgridTopicUpdate(
        AzEventgridTopicUpdateEventgrid eventgrid
    )
    {
        Eventgrid = eventgrid;
    }

    public AzEventgridTopicUpdateEventgrid Eventgrid { get; }
}