using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "topic")]
public class AzEventgridTopicCreate
{
    public AzEventgridTopicCreate(
        AzEventgridTopicCreateEventgrid eventgrid
    )
    {
        Eventgrid = eventgrid;
    }

    public AzEventgridTopicCreateEventgrid Eventgrid { get; }
}