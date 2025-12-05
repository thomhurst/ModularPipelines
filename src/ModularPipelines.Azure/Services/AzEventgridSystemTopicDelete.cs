using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "system-topic")]
public class AzEventgridSystemTopicDelete
{
    public AzEventgridSystemTopicDelete(
        AzEventgridSystemTopicDeleteEventgrid eventgrid
    )
    {
        Eventgrid = eventgrid;
    }

    public AzEventgridSystemTopicDeleteEventgrid Eventgrid { get; }
}