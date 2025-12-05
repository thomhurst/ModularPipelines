using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "system-topic")]
public class AzEventgridSystemTopicCreate
{
    public AzEventgridSystemTopicCreate(
        AzEventgridSystemTopicCreateEventgrid eventgrid
    )
    {
        Eventgrid = eventgrid;
    }

    public AzEventgridSystemTopicCreateEventgrid Eventgrid { get; }
}