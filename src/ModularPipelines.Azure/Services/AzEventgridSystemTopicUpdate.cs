using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "system-topic")]
public class AzEventgridSystemTopicUpdate
{
    public AzEventgridSystemTopicUpdate(
        AzEventgridSystemTopicUpdateEventgrid eventgrid
    )
    {
        Eventgrid = eventgrid;
    }

    public AzEventgridSystemTopicUpdateEventgrid Eventgrid { get; }
}