using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "topic")]
public class AzEventgridTopicDelete
{
    public AzEventgridTopicDelete(
        AzEventgridTopicDeleteEventgrid eventgrid
    )
    {
        Eventgrid = eventgrid;
    }

    public AzEventgridTopicDeleteEventgrid Eventgrid { get; }
}